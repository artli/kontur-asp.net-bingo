using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Filter characters by gender",
                url: "Characters/ByGender/{gender}",
                defaults: new { controller = "Characters", action = "List", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Filter characters by price range",
                url: "Characters/ByPrice/{minPrice}-{maxPrice}",
                defaults: new { controller = "Characters", action = "List", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Show all characters",
                url: "Characters/",
                defaults: new { controller = "Characters", action = "List", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Characters", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}
