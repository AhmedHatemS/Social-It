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
    public class SearchController : DbConnection
    {
        // GET: AspNetUsers
        public ActionResult SearchForFriend(string searchBy, string search)
        {
            List<SearchModel> userInfo = new List<SearchModel>();
            string id = User.Identity.GetUserId();
            ViewBag.id = id;
            ViewBag.searchBy = searchBy; 
            ViewBag.search = search;



            var query = from user in db.AspNetUsers

                        join friend in db.FriendOfUsers on user.Id equals friend.UserId into friendInfo
                        from friendResult in friendInfo.DefaultIfEmpty()

                        join friendRequest in db.FriendRequests on user.Id equals friendRequest.UserId into requestInfo
                        from requestResult in requestInfo.DefaultIfEmpty()

                        
                        orderby user.Id
                        select new
                        {
                            userId = user.Id,
                            firstName = user.FirstName,
                            lastName = user.LastName,
                            image = user.Image,
                            phoneNumber = user.PhoneNumber,
                            email = user.Email,
                            friendId = friendResult.FriendId,
                            requested = requestResult.UserId,
                            requester = requestResult.RequestSenderId,
                        };

            foreach (var view in query)
            {
                if (view.email.StartsWith(search) && searchBy == "Email" && view.userId!=id )
                {
                    userInfo.Add(new SearchModel
                    {
                        ProfileUserId = view.userId,
                        ProfileFirstName = view.firstName,
                        ProfileLastName = view.lastName,
                        ProfileImage = view.image,
                        SearchPhoneNumber = view.phoneNumber,
                        SearchEmail = view.email,
                        ProfilefriendId = view.friendId,
                        ProfileRequest = view.requested,
                        ProfileRequester = view.requester

                    });
                }
                else if ( view.phoneNumber == search && searchBy == "Phone" && view.userId != id)
                {
                    userInfo.Add(new SearchModel
                    {
                        ProfileUserId = view.userId,
                        ProfileFirstName = view.firstName,
                        ProfileLastName = view.lastName,
                        ProfileImage = view.image,
                        SearchPhoneNumber = view.phoneNumber,
                        SearchEmail = view.email,
                        ProfilefriendId = view.friendId,
                        ProfileRequest = view.requested,
                        ProfileRequester = view.requester

                    });
                }
            }
            return View(userInfo);
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
