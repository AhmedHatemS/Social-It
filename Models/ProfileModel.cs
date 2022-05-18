using System.Web;

namespace SocialIt.Models
{
    public class ProfileModel
    {
        public string ProfileUserId { get; set; }
        public string ProfileFirstName { get; set; }
        public string ProfileLastName { get; set; }
        public string ProfileImage { get; set; }
        public string ProfilefriendId { get; set; }
        public string ProfileRequest { get; set; }
        public string ProfileRequester { get; set; }
        public int? ProfilePostId { get; set; }
        public string PostVisibility { get; set; }
    }
}