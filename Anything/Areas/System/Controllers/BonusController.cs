using Anything.Controllers;
using Anything.ViewModels;
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
            var model = new BonusForSysModel();
            var result = model.Query();
            ViewData.Model = model.Query();
            return View();
        }

        [HttpPost]
        public ActionResult Index(BonusForSysModel model)
        {
            if (ModelState.IsValid)
            {
                model.Edit();
            }
            return View();
        }
    }
}