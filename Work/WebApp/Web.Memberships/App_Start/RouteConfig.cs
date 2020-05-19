using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Memberships
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //attribute based routes
            routes.MapMvcAttributeRoutes();

            /*
             We are using attribute based routing for ProductContent Details so have commented the following
             It is here to show as an alternate
            */
            /*
            routes.MapRoute(
                name: "ProductContent",
                url: "{controller}/{action}/{productId}/{itemId}",
                defaults: new { controller = "ProductContent", action = "Details" }
            );
            */

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
