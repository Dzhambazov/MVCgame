using FirebirdWars.Controllers;
using FirebirdWars.Migrations;
using FirebirdWars.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;

namespace FirebirdWars
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/fwlink/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        protected void Application_AuthorizeRequest()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userGuidId = User.Identity.GetUserId();
                BuildingsController bc = new BuildingsController();
                bc.CheckForFinishedBuildings(userGuidId);
                bc.CheckForFinishedUnits(userGuidId);
                bc.UpdateResources(userGuidId);
            }
            
        }
    }
}
