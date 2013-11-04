using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FirebirdWars
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Administration",
               url: "Administration/Administration/{action}",
               defaults: new { controller = "Administration", action = "Index" }
           );


            routes.MapRoute(
               name: "Buildings",
               url: "Buildings/{action}/{id}/{buildingType}",
               defaults: new { controller = "Buildings", action = "Building", buildingType = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
