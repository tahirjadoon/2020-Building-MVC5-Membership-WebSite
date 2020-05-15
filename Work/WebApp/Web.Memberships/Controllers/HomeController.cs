using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Memberships.Extensions;
using Web.Memberships.Models;

namespace Web.Memberships.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new HomeModel();

            /*
             We can get the user's info like following or we'll use the extension we created to get user id. 
             Leaving both here for 
            */
            if (Request.IsAuthenticated)
            {
                //put a break point and run through following ad check each value for more details. 

                //getting from User.Identity
                model.UserIdentityUserId = User.Identity.GetUserName();
                model.UserIdentityFirstName = User.Identity.GetUserFirstName();
                model.UserIdentityLastName = User.Identity.GetUserLastName();
                model.UserIdentityId = User.Identity.GetUserId();

                //getting the id from the HttpContext extension GetUserId
                model.HttpContextId = HttpContext.User.Identity.GetUserId();

                //get all the thumbnails
                var thumbnails = await new List<ThumbnailModel>().GetProductThumbnailsAsync(model.HttpContextId);
                if (thumbnails.Any())
                {
                    //thumbnails per area 
                    var area = 4;
                    var count = thumbnails.Count() / area;
                    var thumnailsArea = new List<ThumbnailAreaModel>();
                    for(int i=0; i<= count; i++)
                    {
                        var areaData = new ThumbnailAreaModel() {
                            Title = i.Equals(0) ? "My Content" : string.Empty,
                            Thumbnails = thumbnails.Skip(i * area).Take(area)
                        };
                        thumnailsArea.Add(areaData);
                    }

                    model.ThumbnailsArea = new List<ThumbnailAreaModel>();
                    model.ThumbnailsArea.AddRange(thumnailsArea);
                }
            }
                
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}