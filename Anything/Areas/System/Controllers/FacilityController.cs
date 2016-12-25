using Anything.Areas.System.Models;
using Anything.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace Anything.Areas.System.Controllers
{
    public class FacilityController : BaseController
    {
        // GET: System/Facility
        public ActionResult Index(int Page =1)
        {
            var model = new FacilityViewModel().Query();
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 10;

            var result = model.ToPagedList(currentPage, PageSize);
            ViewData.Model = result;
            return View();
            
        }

        public ActionResult Edit(int? ID)
        {
            if (ID.HasValue)
            {
                var model = _db.Facility.Where(o => o.ID == ID).Select(o => new FacilityViewModel
                {
                    Created = o.Created.Value,
                    Description = o.Description,
                    Enabled = o.Enabled,
                    ID = o.ID,
                    Show = o.Show.Value,
                    Text = o.Text
                }).FirstOrDefault();
                ViewData.Model = model;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(FacilityViewModel model)
        {
                       
                model.Edit();
                return RedirectToAction("Index");
            
        }
    }
}