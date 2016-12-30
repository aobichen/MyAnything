using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Areas.My.Controllers
{
    public class AccountController : Controller
    {
        // GET: My/Account
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles="User")]
        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}