using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FirebirdWars.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminRestrictController : Controller
    {
    }
}
