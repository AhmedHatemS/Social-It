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
    public class PostCommentsController : DbConnection
    {
       
        // GET: PostComments
        public ActionResult Index(int? id, string userId)
        {

            var postComments = SelectCommentsFromDB(id);

            if (userId == null || postComments == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            ViewBag.currentpostId = id;
            ViewBag.currentUserId = userId;
            return View(postComments);
        }

        private List<PostComment> SelectCommentsFromDB(int? id)
        {
            var userComments = db.PostComments.Where(i => i.PostId == id);
            return userComments.ToList();
        }

        // GET: PostComments/Create
        public ActionResult CreateComment(int postId, string userId)
        {
            ViewBag.UserId = userId;
            ViewBag.PostId = postId;
            return View();
        }

        // POST: PostComments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "Id,PostId,UserId,Comment")] PostComment postComment)
        {
            if (ModelState.IsValid)
            {
                if(addCommentToDatabase(postComment))
                return RedirectToAction("Index", new { id = postComment.PostId, userId = postComment.UserId });
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", postComment.UserId);
            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Content", postComment.PostId);

            return View(postComment);
        }

        private bool addCommentToDatabase(PostComment postcomment)
        {
            db.PostComments.Add(postcomment);
            db.SaveChanges();

            return true;
        }

        // GET: PostComments/Delete/5
        public ActionResult Delete(int? id, int? postId, string userId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostComment postComment = selectOneCommentFromDB(id);

            if (postComment == null || postComment.UserId != userId)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = userId;
            ViewBag.PostId = postId;
            return View(postComment);
        }

        // POST: PostComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostComment postComment = selectOneCommentFromDB(id);
            var currentUserId = postComment.UserId;

            if (deleteCommentToDatabase(postComment))
                return RedirectToAction("Index", new { id = postComment.PostId, userId = currentUserId });
            else
                return HttpNotFound();

        }

        private PostComment selectOneCommentFromDB(int? id)
        {
            var comment = db.PostComments.Find(id);
            return comment;
        }

        private bool deleteCommentToDatabase(PostComment postcomment)
        {

            db.PostComments.Remove(postcomment);
            db.SaveChanges();

            return true;
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
