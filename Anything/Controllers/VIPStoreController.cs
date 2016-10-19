using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    public class VIPStoreController : BaseController
    {
        // GET: VIPStore
        public ActionResult Index()
        {
            var Sys = _db.SysManage.Where(o => o.FieldCode == "VIP" || o.FieldCode == "VIPPrice" || o.FieldCode == "VIPDays").ToList();
            var model = new VIPModel();
            model.VIPDescription = Sys.Where(o => o.FieldCode == "VIP").FirstOrDefault().FieldDescription;
            model.VIPSalePrice = Sys.Where(o => o.FieldCode == "VIPPrice").FirstOrDefault().Value;
            model.SaleDays = Sys.Where(o => o.FieldCode == "VIPDays").FirstOrDefault().Value;
            return View(model);
        }

        public ActionResult Order()
        {
            var model = new VIPsViewModel(3);
            model.BeginDate = DateTime.Now.AddDays(1);
            model.EndDate = DateTime.Now.AddDays(31);
            ViewBag.Hotels = model.Hotels;
            return View(model);
        }
    }
}