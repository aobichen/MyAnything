using Anything.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Areas.Hotel.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Hotel/Account
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles="Hotel")]

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}