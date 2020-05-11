using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Web.Memberships.Models;

namespace Web.Memberships.Extensions
{
    public static class IdentityExtensions
    {
        public static ApplicationUser GetUserInfo(this IIdentity identity)
        {
            var db = ApplicationDbContext.Create(); //Create ==> to get the instance
            var user = db.Users.FirstOrDefault(u => u.UserName.Equals(identity.Name));
            return user;
        }

        public static string GetUserFirstName(this IIdentity identity)
        {
            var user = identity.GetUserInfo();
            return user != null ? user.FirstName : String.Empty;
        }

        public static string GetUserLastName(this IIdentity identity)
        {
            var user = identity.GetUserInfo();
            return user != null ? user.LastName : String.Empty;
        }

        public static string GetUserFullName(this IIdentity identity)
        {
            var user = identity.GetUserInfo();
            return user != null ? $"{user.FirstName} {user.LastName}" : String.Empty;
        }

        public static async Task GetUsers(this List<UserViewModel> users)
        {
            var db = ApplicationDbContext.Create();
            users.AddRange(await (from u in db.Users
                                  select new UserViewModel
                                  {
                                      Id = u.Id,
                                      Email = u.Email,
                                      FirstName = u.FirstName, 
                                      LastName = u.LastName
                                  }).OrderBy(o => o.Email).ToListAsync());
        }
    }
}