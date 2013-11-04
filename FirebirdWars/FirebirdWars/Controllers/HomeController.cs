using FirebirdWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace FirebirdWars.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            string userGuidId = this.User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            if (user != null)
            {
                return View(user);
            }

            return View();
        }
    }
}