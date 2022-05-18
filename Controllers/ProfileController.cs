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
    public class ProfileController : DbConnection
    {

        // GET: Profile
        public ActionResult Index(string id)
        {
            List<ProfileModel> userInfo = new List<ProfileModel>();
            if (id == null)
            {
                id = User.Identity.GetUserId();
            }
            ViewBag.id = id;
            var query = from user in db.AspNetUsers

                        join friend in db.FriendOfUsers on user.Id equals friend.UserId into friendInfo
                        from friendResult in friendInfo.DefaultIfEmpty()

                        join friendRequest in db.FriendRequests on user.Id equals friendRequest.UserId into requestInfo
                        from requestResult in requestInfo.DefaultIfEmpty()

                        join post in db.Posts on user.Id equals post.UserId into postsInfo
                        from postsResult in postsInfo.DefaultIfEmpty()

                        where user.Id == id
                        orderby user.Id
                        select new
                        {
                            userId = user.Id,
                            firstName = user.FirstName,
                            lastName = user.LastName,
                            image = user.Image,
                            friendId = friendResult.FriendId,
                            requested = requestResult.UserId,
                            requester = requestResult.RequestSenderId,
                            postId = (int?)postsResult.PostId,
                            postVisibilty = postsResult.Visibility
                        };

            foreach (var view in query)
            {
                userInfo.Add(new ProfileModel
                {
                    ProfileUserId = view.userId,
                    ProfileFirstName = view.firstName,
                    ProfileLastName = view.lastName,
                    ProfileImage = view.image,
                    ProfilefriendId = view.friendId,
                    ProfileRequest = view.requested,
                    ProfileRequester = view.requester,
                    ProfilePostId = view.postId,
                    PostVisibility = view.postVisibilty
                });
            }
            return View(userInfo);
        }

    }
}