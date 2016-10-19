using Anything.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    [Authorize(Roles="Admin,AdManager")]
    public class AdManageController : BaseController
    {
        // GET: AdManage
        public ActionResult Index()
        {
            var model = _db.AdManage.ToList();
            return View(model);
        }


        [HttpPost]
        public ActionResult Create(AdManage model)
        {
            var Now = DateTime.Now;
            model.Created = Now;
            model.Creator = CurrentUser.Id;
            model.Modified = Now;
            model.Modify = CurrentUser.Id;

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    model.Image = fileData;
                }
            }

            model.Position = _db.CodeFile.Where(o => o.ID == model.PositionId).FirstOrDefault().ItemCode;
            var HasSamePosition = _db.AdManage.Any(o => o.Position == model.Position && o.Enabled == true);
            if (HasSamePosition)
            {
                ModelState.AddModelError("", "廣告區域已被設定");
            }

            if (ModelState.IsValid)
            {

                _db.AdManage.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            

            ViewBag.Position = GetOptions();
            return View(model);
           }

        public ActionResult Create()
        {
            var model = new AdManage();
            model.AllowText = true;
            model.AllowImage = true;
            model.ImageLimit = 1;
            model.TextLimit = 20;
            model.MaxCount = 5;
            model.ImageWidth = 1200;
            model.ImageHeight = 600;
            model.Days = 30;
            ViewBag.Position = GetOptions();
            model.Enabled = true;
            model.AllowImageType = "jpg,jpeg,png";
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var Result = _db.AdManage.Where(o => o.ID == id).FirstOrDefault();
            return View(Result);
        }

        [HttpPost]
        public ActionResult Edit(AdManage model)
        {
            var Result = _db.AdManage.Where(o => o.ID == model.ID).FirstOrDefault();
            Result.Modified = DateTime.Now;
            Result.Modify = CurrentUser.Id;
            if (ModelState.IsValid)
            {
                Result.ImageHeight = model.ImageHeight;
                Result.ImageHeight = model.ImageWidth;
                Result.MaxCount = model.MaxCount;
                Result.SaleOff = model.SaleOff;
                Result.TextLimit = model.TextLimit;
                Result.Enabled = model.Enabled;
                Result.AllowImage = model.AllowImage;
                Result.AllowText = model.AllowText;
                Result.SalePrice = model.SalePrice;
                Result.DiscountPrice = model.DiscountPrice;
                Result.Days = model.Days;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Result);
        }

        
        private SelectList GetOptions()
        {
            var type = "AdPosition";
            var option = _db.CodeFile.Where(o => o.ItemType == type).ToList();
            SelectList selectList = new SelectList(option,"ID","ItemCode");
            return selectList;
        }


    }
}