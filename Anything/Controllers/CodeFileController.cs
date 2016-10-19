using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    [Authorize(Roles="Admin")]
    public class CodeFileController : BaseController
    {
        // GET: CodeFile
        public ActionResult Index()
        {
            var model = _db.CodeFile.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CodeFile model)
        {
            if (ModelState.IsValid)
            {
                _db.CodeFile.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodeType = CodeTypes();
            return View(model);
        }
        public ActionResult Create()
        {
            
           
            ViewBag.CodeType = CodeTypes();
            var model = new CodeFile();
            model.Enabled = true;
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.CodeType = CodeTypes();
            var model = _db.CodeFile.Find(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CodeFile model)
        {
            if (ModelState.IsValid)
            {
                var result = _db.CodeFile.Find(model.ID);
                result.ItemCode = model.ItemCode;
                result.ItemDescription = model.ItemDescription;
                result.ItemType = model.ItemType;
                result.Remark = model.Remark;
                result.TypeText = model.TypeText;
                result.Enabled = model.Enabled;
                result.TypeText = model.TypeText;
            }

            return View(model);
        }

        private SelectList CodeTypes()
        {
            var Options = _db.CodeFile.GroupBy(p => new { Value = p.ItemType, Text = p.TypeText })
                .Select(g => g.FirstOrDefault()).ToList();

            SelectList selectList = new SelectList(Options, "ItemType", "TypeText");
            return selectList;
        }
    }
}