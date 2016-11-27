using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Anything.Helpers;
namespace Anything.Controllers
{
     [Authorize(Roles = "Admin")]
    public class ScenicController : BaseController
    {
        // GET: Scenic
        public ActionResult Index(int Page = 1)
        {
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 10;
            var model = _db.Scenic.ToList().OrderByDescending(o => o.ID);
            var result = model.ToPagedList(currentPage, PageSize);
            return View(result);
        }

        public ActionResult Create()
        {
            var model = new Scenic();
            model.Enabled = true;
            var City = new Caches().TWCity;
           
            SelectList selectList = new SelectList(City, "City", "City", 0);
            ViewBag.City = selectList;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Scenic model)
        {
            
            if (ModelState.IsValid)
            { 
                
                _db.Scenic.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {

            var model = _db.Scenic.Where(o => o.ID == id).FirstOrDefault();
            if (model == null)
            {
                return Redirect("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Scenic model)
        {

            if (ModelState.IsValid)
            {
                var result = _db.Scenic.Where(o => o.ID == model.ID).FirstOrDefault();
                if (result == null)
                {
                    ModelState.AddModelError("", "查無資料");
                    return View();
                }

                result.Name = model.Name;
                result.Description = model.Description;
                result.Enabled = model.Enabled;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}