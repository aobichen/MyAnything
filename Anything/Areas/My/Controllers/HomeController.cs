using Anything.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Areas.My.Controllers
{
    public class HomeController : BaseController
    {
        // GET: My/Home
        public ActionResult Index()
        {
            //var HotelList = from order in _db.OrderMaster join
            //                room in _db.Room on order.ProductId equals room.ID

            return View();
        }
    }
}