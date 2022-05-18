using System.Web;
using System.Collections.Generic;
namespace SocialIt.Models
{
    public class SearchModel
    {
        public string ProfileUserId { get; set; }
        public string ProfileFirstName { get; set; }
        public string ProfileLastName { get; set; }
        public string ProfileImage { get; set; }
        public string SearchPhoneNumber { get; set; }
        public string SearchEmail { get; set; }
        public string ProfilefriendId { get; set; }
        public string ProfileRequest { get; set; }
        public string ProfileRequester { get; set; }
        public int? ProfilePostId { get; set; }
        public string PostVisibility { get; set; }
    }
}