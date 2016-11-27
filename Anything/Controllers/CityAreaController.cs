using Anything.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    public class CityAreaController : BaseController
    {
        // GET: CityArea
        public ActionResult Index()
        {
            var City = new Caches().TWCity;
            ViewBag.Area = new Caches().TWArea;
            SelectList selectList = new SelectList(City, "ID", "City",0);
            ViewBag.City = selectList;
           
            return View();
        }

        public ActionResult TaiwanSelectList()
        {
            var City = new Caches().TWCity;
            ViewBag.Area = new Caches().TWArea;
            SelectList selectList = new SelectList(City, "ID", "City", 0);
            ViewBag.City = selectList;

            return PartialView("_TaiwanSelectList");
        }
    }
}