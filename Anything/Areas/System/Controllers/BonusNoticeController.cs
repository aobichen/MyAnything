using Anything.Areas.System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Areas.System.Controllers
{
    [Authorize(Roles="Admin,System")]
    public class BonusNoticeController : Controller
    {
        // GET: System/BonusNotice
        public ActionResult Index()
        {
            var model = new BonusNoticeViewModel().Query();
            ViewData.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(BonusNoticeViewModel model)
        {
            model.Edit();           
            return RedirectToAction("Index");
        }

        public ActionResult Notice1date()
        {
           
            var model = new BonusNoticeViewModel().QueryFor1Date();
            ViewData.Model = model;
            return View();
        }


        [HttpPost]
        public ActionResult Notice1dateEdit(BonusNoticeViewModel model)
        {

            model.Edit1date();
            return RedirectToAction("Notice1date");
        }
    }
}