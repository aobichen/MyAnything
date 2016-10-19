using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    [Authorize(Roles="Admin")]
    public class SysManageController : BaseController
    {
        // GET: SysManage
        public ActionResult Index()
        {
            var model = _db.SysManage.ToList();
            ViewBag.SysManageList = model;
            return View(model);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SysManage model)
        {
            model.Edited = DateTime.Now;
            model.Editor = CurrentUser.Id;
            
            if (ModelState.IsValid)
            {
                _db.SysManage.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        

        public ActionResult Edit(int id)
        {
            var model = _db.SysManage.Find(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SysManage model)
        {
            var result = _db.SysManage.Find(model.ID);
            if (result == null)
            {
                return RedirectToAction("Index");
            }

            result.Value = model.Value;
            result.FieldName = model.FieldName;
            result.FieldDescription = model.FieldDescription;
            result.Enabled = model.Enabled;
            result.Edited = DateTime.Now;
            result.Editor = 1;
            _db.SaveChanges();

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(List<SysManage> Model)
        {
            return View();
        }
    }
}