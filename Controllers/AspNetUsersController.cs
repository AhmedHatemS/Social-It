using System;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocialIt.Models;

namespace SocialIt.Controllers
{
    [Authorize]
    public class AspNetUsersController : Controller
    {
        private SocialIt_DBEntities db = new SocialIt_DBEntities();

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET:
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST:
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Gender,Image,Country,City,DateOfBirth,FirstName,LastName")] AspNetUser aspNetUser, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                if (imgFile != null)
                {
                    EditProfileImage(aspNetUser, imgFile);
                }
                aspNetUser.UserName = aspNetUser.Email;
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "AspNetUsers", new { id = aspNetUser.Id });
            }
            return View(aspNetUser);
        }

        private void EditProfileImage(AspNetUser aspNetUser, HttpPostedFileBase imgFile)
        {
            if (imgFile.ContentLength > 0)
            {
                if (Path.GetExtension(imgFile.FileName) == ".PNG" ||
                    Path.GetExtension(imgFile.FileName) == ".Png" ||
                    Path.GetExtension(imgFile.FileName) == ".png")
                {
                    string imgPath = "";
                    var timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

                    imgPath = "~/Content/Images/ProfilePics/" + timeStamp 
                        + Path.GetExtension(imgFile.FileName);
                    imgFile.SaveAs(Server.MapPath(imgPath));
                    aspNetUser.Image = "" + timeStamp;
                }
                else
                {
                    aspNetUser.Image = "2";
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
