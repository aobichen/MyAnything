using Anything.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Areas.System.Controllers
{
    public class BonusController : BaseController
    {
        // GET: System/Bonus
        public ActionResult Index()
        {
            return View();
        }
    }
}