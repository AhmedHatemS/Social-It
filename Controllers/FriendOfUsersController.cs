using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocialIt.Models;
using Microsoft.AspNet.Identity;


namespace SocialIt.Controllers
{
    [Authorize]
    public class FriendOfUsersController : DbConnection
    {
        // GET: FriendOfUsers
        public ActionResult ShowAllFriends()
        {

            var userFriends = SelectFriendsFromDB();
            return View(userFriends);
        }

        private List<FriendOfUsersModel> SelectFriendsFromDB()
        {
            List<FriendOfUsersModel> userFriends = new List<FriendOfUsersModel>();
            var myId = User.Identity.GetUserId();
            var query = from friends in db.FriendOfUsers
                        join user in db.AspNetUsers
                        on friends.FriendId equals user.Id
                        where friends.UserId == myId
                        select new
                        {
                            friendId = friends.FriendId,
                            firstName = user.FirstName,
                            lastName = user.LastName,
                            image = user.Image,
                        };

            foreach (var friend in query)
            {
                userFriends.Add(new FriendOfUsersModel { FriendId = friend.friendId, FirstName = friend.firstName, LastName = friend.lastName, Image = friend.image });
            }
            return userFriends;
        }
        

        private void RemoveFriendsFromDB(FriendOfUser removeMeFromOtherFriendList, FriendOfUser removeOtherFriendFromMyList)
        {
            db.FriendOfUsers.Remove(removeMeFromOtherFriendList);
            db.FriendOfUsers.Remove(removeOtherFriendFromMyList);
        }
        public ActionResult UnFriend(string id)
        {
            string friendId = id;
            string myId = User.Identity.GetUserId();
            if (friendId == null)
            {
                return HttpNotFound();
            }
            FriendOfUser removeMeFromOtherFriendList = db.FriendOfUsers.FirstOrDefault(f => f.FriendId == myId && f.UserId == friendId);
            FriendOfUser removeOtherFriendFromMyList = db.FriendOfUsers.FirstOrDefault(f => f.FriendId == friendId && f.UserId == myId);

            RemoveFriendsFromDB(removeMeFromOtherFriendList, removeOtherFriendFromMyList);
            db.SaveChanges();

            return RedirectToActionPermanent("Index", "Profile", new { id = friendId });
           
            
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
