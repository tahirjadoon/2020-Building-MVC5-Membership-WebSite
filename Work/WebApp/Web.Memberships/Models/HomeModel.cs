using System.Collections.Generic;

namespace Web.Memberships.Models
{
    public class HomeModel
    {
        public List<ThumbnailAreaModel> ThumbnailsArea { get; set; }

        //following fields pulled via User.Identity in Home Controller Index action 
        public string UserIdentityUserId { get; set; }

        public string UserIdentityFirstName { get; set; }

        public string UserIdentityLastName { get; set; }

        public string UserIdentityId { get; set; }

        public string DisplayUserIdentityName => $"{UserIdentityFirstName} {UserIdentityLastName}";

        //using the HttpContent GetuserId
        public string HttpContextId { get; set; }
    }
}