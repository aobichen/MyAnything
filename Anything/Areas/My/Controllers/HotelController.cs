using Anything.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Areas.My.Controllers
{
    [Authorize]
    public class HotelController : BaseController
    {
        // GET: My/Hotel
        public ActionResult Index()
        {
            var result = _db.Hotel.Where(o => o.UserId == CurrentUser.Id);
            ViewData.Model = result;
            return View();
        }
    }
}