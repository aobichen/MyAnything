using Anything.Controllers;
using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Anything.Areas.System.Models;
namespace Anything.Areas.System.Controllers
{
    public class BedTypeController : BaseController
    {
        // GET: System/BedType
        public ActionResult Index(int Page=1)
        {
            var model = new BedTypeModel().Query();
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 10;
            
            var result = model.ToPagedList(currentPage, PageSize);
            ViewData.Model = result;
            return View();
        }
    }
}