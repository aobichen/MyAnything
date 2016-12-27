using Anything.Areas.System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Anything.Controllers;
namespace Anything.Areas.System.Controllers
{
     [Authorize(Roles = "Admin,System")]
    public class RoomTypeController : BaseController
    {
        // GET: System/RoomType
         public ActionResult Index(int Page = 1)
        {
            var model = new RoomTypeViewModel().Query();
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 10;

            var result = model.ToPagedList(currentPage, PageSize);
            ViewData.Model = result;
            return View();
        }

         public ActionResult Edit(int? id)
         {
             if (id.HasValue)
             {
                 var model = new RoomTypeViewModel().Single(id.Value);
                 ViewData.Model = model;
                 return View();
             }
             return View();
         }

         [HttpPost]
         public ActionResult Edit(RoomTypeViewModel model)
         {

             model.Edit();
             return RedirectToAction("Index");
         }
    }
}