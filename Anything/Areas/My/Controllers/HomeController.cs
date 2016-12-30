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
    [Authorize]
    public class HomeController : BaseController
    {
        // GET: My/Home
        public ActionResult Index(string Searchkey,int Page = 1)
        {
            //var HotelList = from order in _db.OrderMaster join
            //                room in _db.Room on order.ProductId equals room.ID
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 25;
            if (User.IsInRole("Hotel"))
            {
                var RoomId = (from hotel in _db.Hotel
                              join room in _db.Room
                                  on hotel.ID equals room.HotelId
                              where hotel.UserId == CurrentUser.Id
                              select room.ID).ToList();
                var HotelOrder = new HotelOrderModel();
                HotelOrder.RoomID = RoomId;
                HotelOrder.SearchKey = Searchkey;
                var HotelOrders = HotelOrder.List();
                
            

            var PageModel = HotelOrders.ToPagedList(currentPage, PageSize);
            ViewBag.HotelOrders = PageModel;
            }

            var cur = CurrentUser.Id;
            if (User.IsInRole("User"))
            {

                var MyOrder = (from order in _db.OrderMaster
                               join room in _db.Room
                                   on order.ProductId equals room.ID
                               join hotel in _db.Hotel
                               on room.HotelId equals hotel.ID
                               where order.UserId == CurrentUser.Id
                               select new MyOrderModel
                               {
                                   Total = order.Amount,
                                   PayBonus = order.BonusAmt,
                                   PayAmt = order.PayAmt,
                                   HotelName = hotel.Name,
                                   CheckInDate = order.CheckIn,
                                   CheckOutDate = order.CheckOut,
                                   Created = order.Created,
                                   ID = order.ID,
                                   MerchantOrderNo = order.MerchantOrderNo,
                                   RoomName = room.Name,
                                   PaymentType = order.PaymentType,
                                   PayStatus = order.Status

                               }).OrderByDescending(o => o.Created).ToList();




                var MyOrderPageModel = MyOrder.ToPagedList(currentPage, PageSize);
                ViewBag.MyOrders = MyOrderPageModel;
            }
            return View();
        }
    }
}