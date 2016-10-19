using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    [Authorize]
    public class MyOrderController : BaseController
    {
        // GET: MyOrder
        public ActionResult Index()
        {
            var OrderOfHotel = (from order in _db.OrderMaster
                                join room in _db.Room
                                    on order.ProductId equals room.ID
                                where order.ProductType == "Room" && 
                                order.UserId == CurrentUser.Id
                                select new MyOrderOfHotel
                                {
                                    ID = order.ID,
                                    CheckInDate = order.CheckIn,
                                    CheckOutDate = order.CheckOut,
                                    HotelId = room.Hotel.ID,
                                    HotelName = room.Hotel.Name,
                                    RoomId = room.ID,
                                    RoomName = room.Name,
                                    Price = order.Price,
                                    Amount = order.Amount,
                                    Total = order.Total,
                                    PayStatus = order.TradeStatus,
                                    Created = order.Created
                                }).OrderBy(o=>o.Created).ToList();
            ViewBag.OrderOfHotel = OrderOfHotel;               
            return View();
        }
    }
}