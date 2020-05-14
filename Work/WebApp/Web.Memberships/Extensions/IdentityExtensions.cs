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

            //1: using two queries in a join
            var subQuery = from us in db.UserSubscriptions
                           join s in db.Subscriptions on us.SubscriptionId equals s.Id
                           group us by us.UserId into countGroup
                           select new { Count = countGroup.Count(), UserId = countGroup.Key };

            var query = from u in db.Users
                        join sq in subQuery on u.Id equals sq.UserId into sq1 
                        from sqR in sq1.DefaultIfEmpty()
                        select new UserViewModel()
                        {
                            Id = u.Id,
                            Email = u.Email,
                            FirstName = u.FirstName,
                            LastName = u.LastName, 
                            SubscriptionsCount = sqR.Count == null ? 0 : sqR.Count
                        };

            var siteUsers = await(query).OrderBy(o => o.Email).ToListAsync();

            if(siteUsers != null && siteUsers.Any())
            {
                users.AddRange(siteUsers);
            }

            //2: using a sub query
            /*
            var singleQuery = from u in db.Users 
                              join sub in (from us in db.UserSubscriptions  
                                           join s in db.Subscriptions on us.SubscriptionId equals s.Id 
                                           group us by us.UserId into countGroup
                                           select new { Count = countGroup.Count(), UserId = countGroup.Key }) 
                                on u.Id equals sub.UserId into sub1 
                              from subR in sub1.DefaultIfEmpty() 
                              select new UserViewModel
                              {
                                  Id = u.Id,
                                  Email = u.Email,
                                  FirstName = u.FirstName,
                                  LastName = u.LastName, 
                                  SubscriptionsCount = subR.Count == null ? 0 : subR.Count
                              };

            var siteUsersSub = await (query).OrderBy(o => o.Email).ToListAsync();

            if (siteUsersSub != null && siteUsersSub.Any())
            {
                users.AddRange(siteUsersSub);
            }
            */

            //3. using two independent queries
            /*
            users.AddRange(await (from u in db.Users 
                                  select new UserViewModel
                                  {
                                      Id = u.Id,
                                      Email = u.Email,
                                      FirstName = u.FirstName, 
                                      LastName = u.LastName
                                  }).OrderBy(o => o.Email).ToListAsync());

            if (users.Any())
            {
                foreach(var user in users)
                {
                    user.SubscriptionsCount = await (from us in db.UserSubscriptions
                                                     join s in db.Subscriptions on us.SubscriptionId equals s.Id
                                                     where us.UserId.Equals(user.Id)
                                                     select us).CountAsync();
                }
            }
            */
        }
    }
}