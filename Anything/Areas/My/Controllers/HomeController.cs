using Anything.Areas.My.Models;
using Anything.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace Anything.Areas.My.Controllers
{
    public class HomeController : BaseController
    {
        // GET: My/Home
        public ActionResult Index(string Searchkey,int Page = 1)
        {
            //var HotelList = from order in _db.OrderMaster join
            //                room in _db.Room on order.ProductId equals room.ID
            var RoomId = (from hotel in _db.Hotel
                          join room in _db.Room
                              on hotel.ID equals room.HotelId
                          where hotel.UserId == CurrentUser.Id
                          select room.ID).ToList();
            var HotelOrder = new HotelOrderModel();
            HotelOrder.RoomID = RoomId;
            HotelOrder.SearchKey = Searchkey;
            var HotelOrders = HotelOrder.List();
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 2;

            var PageModel = HotelOrders.ToPagedList(currentPage, PageSize);
            ViewBag.HotelOrders = PageModel;
           
            return View();
        }
    }
}