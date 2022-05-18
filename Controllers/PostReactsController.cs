using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocialIt.Models;

namespace SocialIt.Controllers
{
    public class PostReactsController : DbConnection
    {

        // GET: PostReacts/Details/5
        public ActionResult Details(int? id)
        {
            var postId = id;            
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var postReacts = GetPostReacts(postId);
            if (postReacts == null)
            {
                return HttpNotFound();
            }
            return PartialView(postReacts);
        }

        private List<UserReactModel> GetPostReacts(int? postId)
        {
            List<UserReactModel> postReacts = new List<UserReactModel>();
            var query = from react in db.PostReacts
                        join user in db.AspNetUsers
                        on react.UserId equals user.Id
                        where react.PostId == postId
                        select new
                        {
                            firstName = user.FirstName,
                            lastName = user.LastName,
                            react = react.React
                        };
            foreach (var react in query)
            {
                postReacts.Add(new UserReactModel { FirstName = react.firstName, LastName = react.lastName, React = react.react });
            }
            return postReacts;
        }

        // GET: PostReacts/Create
        public ActionResult Create(int postId, string userId, bool react)
        {
            PostReact postReact = new PostReact
            {
                PostId = postId,
                UserId = userId,
                React = react,
            };
            if (ModelState.IsValid)
            {                
                var reactToDelete = db.PostReacts.SingleOrDefault(row => row.UserId == userId && row.PostId == postId);
                if (reactToDelete != null) //Delete old react for this user on the same post
                {
                    db.PostReacts.Remove(reactToDelete);
                    db.SaveChanges();
                }
                else //Add new react
                {
                    db.PostReacts.Add(postReact);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View();
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
