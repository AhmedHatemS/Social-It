using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using SocialIt.Models;
using Microsoft.AspNet.Identity;

namespace SocialIt.Controllers
{
    [Authorize]
    public class PostsController : DbConnection
    {

        // GET: Posts/Details/5
        public ActionResult Details(int? id, string myId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.myId = User.Identity.GetUserId();
            ViewBag.postUserId = post.UserId;
            return View(post);
        }

 
        public String GetUserId()
        {
            var id="";
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
               .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    id= userIdClaim.Value;
                }
            }

            return id;
        }
        public int GetPostId([Bind(Include = "Content,Visibility")] Post post)
        {
            var id = db.Posts
               .Where(x => x.PostId == post.PostId)
               .Select(x => x.PostId)
               .FirstOrDefault();
            return id;
        }
        // GET: Posts/Create
        public ActionResult CreatePost()
        {
            return View();
        }
        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost([Bind(Include = "Content,Visibility")] Post post)
        {
            if (post == null )
            {
                return HttpNotFound();
            }
            else
            {
                post.UserId = GetUserId();
                post.Time = DateTime.Now;
                AddPostToDatabase(post);
                SavePostToDatabase(post);
                return RedirectToActionPermanent("Index", "Home");
            }


        }
        //Add post 
        public void AddPostToDatabase([Bind(Include = "Content,Visibility")] Post post)
        {
            if (post.Visibility == null)
            {
                post.Visibility = "Public";
            }
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
   

            }
        }
        //Save post in database
        public void SavePostToDatabase([Bind(Include = "Content,Visibility")] Post post)
        {
            if (ModelState.IsValid && post.Content !=null && post.Visibility !=null)
            {
               
                db.SaveChanges();

            }

        }
        // GET: Posts/Edit/5
        public ActionResult EditPost(int? id, string IdOfPost)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdOfPost = IdOfPost;
            return View(post);

        }
        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "Content,Visibility")] Post post)
        {
            
            post.UserId = GetUserId();
         
            var PostId = GetPostId(post);
         
            post.Time = DateTime.Now;
           
            UpdatePostToDatabase(post,PostId);
            
            SavePostToDatabase(post);
       
            return RedirectToActionPermanent("Index", "Home");
        }
        //UpdatePost
        public void UpdatePostToDatabase([Bind(Include = "Content,Visibility")] Post post,int PostId)
        {
            if (ModelState.IsValid)
            {
                if (PostId != null)
                {
                    UpdateModel(post);
                    db.Posts.Attach(post);
                    db.Entry(post).State = EntityState.Modified;
                }
            }
        }

        // GET: Posts/Delete/5
        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            SavePostToDatabase(post);
            return RedirectToAction("Index");
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
