using Anything.Models;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net.Http;

namespace Anything.Controllers
{
    [Authorize(Roles="Admin")]
    public class SystemAdminController : BaseController
    {

        [HttpPost]
        public ActionResult UpdateSaleOff(Hotel model)
        {
            var a = model;
            if (!User.IsInRole("Admin"))
            {
                var resp = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("NotFound Authorize")),
                    ReasonPhrase = "Product ID Not Found"
                };
                throw new System.Web.Http.HttpResponseException(resp);
            }

            var hotel = _db.Hotel.Find(model.ID);
            if (hotel == null)
            {
                var resp = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("NotFound Data")),
                        ReasonPhrase = "Product ID Not Found"
                    };
                    throw new System.Web.Http.HttpResponseException(resp);
            }

            hotel.SaleOff = model.SaleOff;
            _db.SaveChanges();
            return Json(new { message = "success" });
        }
        //
        // GET: /SystemAdmin/
        public ActionResult Index(string SearchString = "", int Page = 1)
        {
            var model = (from h in _db.Hotel
                         where string.IsNullOrEmpty(SearchString) || h.Name.Contains(SearchString)
                         select new HotelsViewModel
                         { 
                             ID = h.ID,
                             Name=h.Name,
                             SaleOff = h.SaleOff,
                             Location = h.Location,                    
            
                                    }).ToList().OrderBy(o => o.ID);
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 20;
           
            var result = model.ToPagedList(currentPage, PageSize);

            //var currentPage = Page < 1 ? 1 : Page;
            //var PageSize = 10;
            //var model = _db.Scenic.ToList().OrderByDescending(o => o.ID);
            //var result = model.ToPagedList(currentPage, PageSize);
            return View(result);
        }

        public ActionResult ServiceOptionCreate()
        {

            var model = new ServiceOption();
            model.Enabled = true;
            return View(model);
        }

        [HttpPost]
        public ActionResult ServiceOptionCreate(ServiceOption model)
        {
            var text = model.Text.Trim();
            var IsExist = _db.ServiceOption.Any(o => o.Text == text);
            if (IsExist)
            {
                ModelState.AddModelError("", "項目已存在");
                return View();
            }

            if (ModelState.IsValid)
            {
                _db.ServiceOption.Add(new Models.ServiceOption { Text = text, Description = model.Description, Enabled = model.Enabled, Created = DateTime.Now });
                _db.SaveChanges();
            }
            return RedirectToAction("ServiceOption");
        }

       
        public ActionResult ServiceOptionEdit(int id)
        {
            var model = _db.ServiceOption.Where(o => o.ID == id).FirstOrDefault();
           
            return View(model);
        }

        [HttpPost]
        public ActionResult ServiceOptionEdit(ServiceOption model)
        {
            var result = _db.ServiceOption.Find(model.ID);
            if (result==null)
            {
                ModelState.AddModelError("","查無資料");
                return View(model);
            }

            model.Created = result.Created;

            if (ModelState.IsValid)
            {
                _db.Entry(result).CurrentValues.SetValues(model);
                _db.SaveChanges();
            }
            return View(model);
        }


        public ActionResult ServiceOption(int Page=1)
        {
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 10;
            var model = _db.ServiceOption.ToList().OrderByDescending(o => o.Created);
            var result = model.ToPagedList(currentPage, PageSize);

            return View(result);
        }

        [HttpPost]
        public ActionResult ServiceOption(ServiceOptionViewModel model)
        {
            
            _db.ServiceOption.Add(new Models.ServiceOption { Text = model.Text, Description = model.Description, Enabled = model.Enabled, Created = DateTime.Now });
            _db.SaveChanges();

            model.Options = _db.ServiceOption.ToList();
            return View(model);
        }

        //public ActionResult ServiceOption2()
        //{
        //    var models = _db.ServiceOption.ToList();


        //    return View(models);
        //}
	}
}