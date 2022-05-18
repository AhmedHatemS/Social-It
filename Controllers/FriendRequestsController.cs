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
    public class FriendRequestsController : DbConnection
    {       
         // GET: FriendRequests
        public ActionResult ShowAllFriendRequest()
        {
            var userFriendRequests = SelectFriendRequstsFromDB();
            return View(userFriendRequests);
        }

        private List<UserFriendRequestModel> SelectFriendRequstsFromDB()
        {
            List<UserFriendRequestModel> userFriendRequests = new List<UserFriendRequestModel>();
            var myId = User.Identity.GetUserId();
            var query = from friendRequests in db.FriendRequests
                        join user in db.AspNetUsers
                        on friendRequests.RequestSenderId equals user.Id
                        where friendRequests.UserId == myId
                        select new
                        {
                            requestSenderId = friendRequests.RequestSenderId,
                            firstName = user.FirstName,
                            lastName = user.LastName,
                            image = user.Image,
                            time = friendRequests.Time
                        };
            foreach (var request in query)
            {
                userFriendRequests.Add(new UserFriendRequestModel { RequestSenderId = request.requestSenderId, FirstName = request.firstName, LastName = request.lastName, Image = request.image, Time = request.time });
            }
            return userFriendRequests;
        }
        private void RemoveFromFriendRequestsDB(FriendRequest friendRequest)
        {
            db.FriendRequests.Remove(friendRequest);
        }
       
        private void AddInFriendRequestTable(string requestSenderId, string myId, DateTime Time)
        {
            db.FriendRequests.Add(new FriendRequest { UserId = requestSenderId, RequestSenderId = myId, Time = Time });
        }
        private void AddFriendsToEachOther(string requestSenderId, string myId)
        {
            db.FriendOfUsers.Add(new FriendOfUser { UserId = myId, FriendId = requestSenderId });
            db.FriendOfUsers.Add(new FriendOfUser { UserId = requestSenderId, FriendId = myId });
        }

        public ActionResult AcceptRequest(string id)
        {
            string requestSenderId = id;
            string myId = User.Identity.GetUserId();
            if (requestSenderId == null)
            {
                return HttpNotFound();
            }
            FriendRequest friendRequest = db.FriendRequests.FirstOrDefault(f => f.RequestSenderId == requestSenderId && f.UserId == myId);
            RemoveFromFriendRequestsDB(friendRequest);
            AddFriendsToEachOther(requestSenderId, myId);
            db.SaveChanges();
            return RedirectToActionPermanent("ShowAllFriendRequest", "FriendRequests");
        }




        public ActionResult SendRequest(string id)
        {
            string requestSenderId = id;
            string myId = User.Identity.GetUserId();
            DateTime Time = DateTime.Now;
            if (requestSenderId == null)
            {
                return HttpNotFound();
            }
            AddInFriendRequestTable(requestSenderId, myId, Time);
            db.SaveChanges();
            return RedirectToActionPermanent("Index", "Profile", new { id = requestSenderId });
        }



        public ActionResult RejectRequest(string id)
        {
            string requestSenderId = id;
            string myId = User.Identity.GetUserId();
            if (requestSenderId == null)
            {
                return HttpNotFound();
            }
            FriendRequest friendRequest = db.FriendRequests.FirstOrDefault(f => f.RequestSenderId == requestSenderId && f.UserId == myId);
            RemoveFromFriendRequestsDB(friendRequest);
            db.SaveChanges();
            return RedirectToActionPermanent("ShowAllFriendRequest", "FriendRequests");
        }
        public ActionResult CancelRequest(string id)
        {
            string requestSenderId = id;
            string myId = User.Identity.GetUserId();
            if (requestSenderId == null)
            {
                return HttpNotFound();
            }
            FriendRequest friendRequest = db.FriendRequests.FirstOrDefault(f => f.RequestSenderId == myId && f.UserId == requestSenderId);
            RemoveFromFriendRequestsDB(friendRequest);
            db.SaveChanges();
            return RedirectToActionPermanent("Index", "Profile", new { id = requestSenderId });
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
