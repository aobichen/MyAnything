using System.Configuration;
using System.Web.Mvc;
using System.Linq;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using Anything.Helper;
using Anything.Models;
namespace Anything.Controllers
{
    public class HomeController : BaseController
    {
        
    
        public ActionResult Index(HomeSearchViewModel model = null,string username = null)
        {
            var Now = DateTime.Now;
            Session["CheckInDate"] = Now;
            Session["CheckOutDate"] = Now.AddDays(1);
            if (model != null && model.BeginDate >= DateTime.Now)
            {
                Session["CheckInDate"] = model.BeginDate;
                Session["CheckOutDate"] = model.EndDate;
            }
            var result = new HotelListViewModel().GetHotels(model);
            ViewBag.HotelList = result;

            var oo = _db.ServiceOption.Where(o => o.Show == true).ToList();
            ViewBag.Options = _db.ServiceOption.Where(o => o.Show == true).ToList();
            var City = new Caches().TWCity;
           
            SelectList selectList = new SelectList(City, "ID", "City", 0);
            ViewBag.City = selectList;
           
            return View();
        }


        public ActionResult AdImage(int id)
        {
            var imageData = _db.AdImage.Find(id).Image;
           return File(imageData, "image/jpeg");
        }
        
        

        public ActionResult Detail(int id)
        {
            
            var model = _db.Hotel.Where(o => o.ID == id).Select(o =>
                new HotelDetail
                {
                    ID = o.ID,
                    Address = o.Address,
                    options = o.ServiceOptions,
                    Images = o.HotelImage.ToList(),
                    Name = o.Name,
                    Feature = o.Feature,
                    Infomation = o.Information,
                    Tel = o.Tel,
                    Scenics = o.Scenics,
                    City = o.City,
                    Area = o.Area
                }).FirstOrDefault();

            var Facilities = model.options.Split(',').Select(int.Parse).ToList();
            model.Facilities = _db.ServiceOption.Where(o => Facilities.Contains(o.ID)).Select(p => p.Text).ToList();

            var Date = Session["CheckInDate"] == null ? DateTime.Now:(DateTime)Session["CheckInDate"];
            var DayOfWeek = Date.DayOfWeek.ToString("d");
            var IsHoliday = false;
            if (DayOfWeek == "5" || DayOfWeek == "6")
            {
                IsHoliday = true;
            }

            model.Rooms = (from r in _db.Room
                           join code in _db.CodeFile
                               on r.BedType equals code.ID
                               join code2 in _db.CodeFile
                               on r.RoomType equals code2.ID
                               where r.HotelId == model.ID
                           select new RoomModel
                           {  ID=r.ID,
                               BedType = code.ItemDescription,
                               Feature = r.Feature,
                               Images = r.RoomImage.ToList(),
                               Name = r.Name,
                               FixedPrice = r.FixedPrice,
                               HolidayPrice = r.HolidayPrice,
                               Quantity = r.Quantity,
                               RoomType = code2.ItemDescription,
                               DayPrice = IsHoliday ? r.HolidayPrice : r.DayPrice,
                               BedAmount = r.BedAmount,
                               Amt = _db.OrderMaster.Where(o => o.ProductId == r.ID && 
                ((o.CheckIn.Year == Date.Year&&
                o.CheckIn.Month == Date.Month&&
                o.CheckIn.Date == Date.Date)||
                (o.CheckOut.Year == Date.Year &&
                o.CheckOut.Month == Date.Month &&
                o.CheckOut.Date == Date.Date))).Sum(o => o.Amount)
                           }).ToList();

            model.Rooms = model.Rooms.Where(o => o.Quantity > o.Amt).ToList();

            ViewBag.NearHotels = _db.Hotel.Where(o => o.City == model.City && o.ID != id).OrderBy(o => Guid.NewGuid()).Take(5).ToList();
            var sce = model.Scenics.Split(',').Select(int.Parse).ToList();
            ViewBag.Scenics = _db.Scenic.Where(o => sce.Contains(o.ID)).Select(o => o.Name).ToList(); ;
            return View(model);
        }


        [Authorize]
        public ActionResult Booking(int id)
        {
           
            var Room = _db.Room.Find(id);
            var CheckInDate = Session["CheckInDate"] == null ? DateTime.Now : (DateTime)Session["CheckInDate"];
            var CheckOutDate = Session["CheckOutDate"] == null ? CheckInDate.AddDays(2): (DateTime)Session["CheckOutDate"];
            var DateSpans = new TimeSpan(CheckOutDate.Ticks - CheckInDate.Ticks).Days;
            var Dates = new List<DateTime>();
            for (var i = 0; i < DateSpans; i++)
            {
                Dates.Add(CheckInDate.AddDays(i));
            }

            var MaxDate = CheckInDate.AddDays(10);

            var Prices = new List<DatePrices>();
            decimal Total = 0;
            #region
            for (DateTime date = CheckInDate; MaxDate.CompareTo(date) > 0; date = date.AddDays(1.0))
            {
                var d = (int)date.DayOfWeek;
                if (d == 5 || d==6)
                {
                    var price = Room.HolidayPrice;
                    var PriceFrom = _db.RoomPrice.Where(o => (o.Date.Year == date.Year && o.Date.Month == date.Month && o.Date.Day == date.Day)
                             && o.ROOMID == Room.ID).FirstOrDefault();
                    if (PriceFrom != null)
                    {
                        switch (PriceFrom.DayType)
                        {
                            case "0":
                                price = Room.DayPrice;
                                break;
                            case "1":
                                price = Room.HolidayPrice;
                                break;
                            case "2":
                                price = Room.FixedPrice;
                                break;
                        }
                       
                    }

                    var Checked = Dates.Where(o => o.Year == date.Year && o.Month == date.Month && o.Date == date.Date).Any();

                    if (Checked)
                    {
                        Total += price;
                    }

                    Prices.Add(new DatePrices { Date = date.ToShortDateString(), Price = price.ToString("#,##0"), Checked = Checked });
                }
                else
                {
                    var price = Room.DayPrice;
                    var PriceFrom = _db.RoomPrice.Where(o => (o.Date.Year == date.Year && o.Date.Month == date.Month && o.Date.Day == date.Day)
                            && o.ROOMID == Room.ID).FirstOrDefault();
                    if (PriceFrom != null)
                    {
                        switch (PriceFrom.DayType)
                        {
                            case "0":
                                price = Room.DayPrice;
                                break;
                            case "1":
                                price = Room.HolidayPrice;
                                break;
                            case "2":
                                price = Room.FixedPrice;
                                break;
                        }
                       
                    }

                    var Checked = Dates.Where(o => o.Year == date.Year && o.Month == date.Month && o.Date == date.Date).Any();
                    if (Checked)
                    {
                        Total += price;
                    }
                    Prices.Add(new DatePrices { Date = date.ToShortDateString(), Price = price.ToString("#,##0"), Checked = Checked });
                }
            }
            ViewBag.PriceList = Prices;
            #endregion

            var Booking = new BookingModel();
            Booking.Address = Room.Hotel.Address;
            Booking.BedType = _db.CodeFile.Find(Room.BedType).ItemDescription + "X" +Room.BedAmount;
            Booking.Name = Room.Name;
            Booking.ID = Room.ID;
            Booking.MaxPeople = Room.MaxPerson;
            Booking.Tel = Room.Hotel.Tel;
            Booking.Total = Total;
            Booking.CheckInDate = CheckInDate;
            Booking.CheckOutDate = CheckOutDate;
            Booking.RoomType = _db.CodeFile.Find(Room.RoomType).ItemDescription ;
            
            return View(Booking);
        }

        [HttpPost]
        public ActionResult Booking(BookingModel model)
        {
            var Room = _db.Room.Find(model.ID);
            var Filled = Room.Quantity >= _db.OrderMaster.Where(o => o.ProductId == Room.ID && 
                ((o.CheckIn.Year == model.CheckInDate.Year&&
                o.CheckIn.Month == model.CheckInDate.Month&&
                o.CheckIn.Date == model.CheckInDate.Date)||
                (o.CheckOut.Year == model.CheckInDate.Year &&
                o.CheckOut.Month == model.CheckInDate.Month &&
                o.CheckOut.Date == model.CheckInDate.Date))).Sum(o => o.Amount);
            if (Filled)
            {
                ModelState.AddModelError("","客滿");
                return View();
            }
            var Dates = model.DateList.Split(',').Select(DateTime.Parse).ToList();
            var CheckInDate = Dates.First();
            var CheckOutDate = Dates.Last();
            decimal Total = 0;
            #region
            for (DateTime date = CheckInDate; CheckOutDate.CompareTo(date) >= 0; date = date.AddDays(1.0))
            {
                var d = (int)date.DayOfWeek;
                if (d == 5 || d == 6)
                {
                    var price = Room.HolidayPrice;
                    var PriceFrom = _db.RoomPrice.Where(o => (o.Date.Year == date.Year && o.Date.Month == date.Month && o.Date.Day == date.Day)
                             && o.ROOMID == Room.ID).FirstOrDefault();
                    if (PriceFrom != null)
                    {
                        switch (PriceFrom.DayType)
                        {
                            case "0":
                                price = Room.DayPrice;
                                break;
                            case "1":
                                price = Room.HolidayPrice;
                                break;
                            case "2":
                                price = Room.FixedPrice;
                                break;
                        }

                    }

                   
                        Total += price;
                   
                }
                else
                {
                    var price = Room.DayPrice;
                    var PriceFrom = _db.RoomPrice.Where(o => (o.Date.Year == date.Year && o.Date.Month == date.Month && o.Date.Day == date.Day)
                            && o.ROOMID == Room.ID).FirstOrDefault();
                    if (PriceFrom != null)
                    {
                        switch (PriceFrom.DayType)
                        {
                            case "0":
                                price = Room.DayPrice;
                                break;
                            case "1":
                                price = Room.HolidayPrice;
                                break;
                            case "2":
                                price = Room.FixedPrice;
                                break;
                        }

                    }

                    Total += price;
                }
            }
            //ViewBag.PriceList = Prices;
            #endregion

            var PayGo = new PayGoRequest();

            PayGo.MerchantOrderNo = Guid.NewGuid().GetHashCode().ToString("x");
            PayGo.LangType = "zh-tw";
            var Now = DateTime.Now;
            PayGo.TimeStamp = DateTime.UtcNow.Subtract(Now).TotalSeconds.ToString();
            PayGo.Amt = int.Parse(Total.ToString());
            PayGo.Version = "1.2";
            PayGo.TradeLimit = 60;
            PayGo.ItemDesc = string.Format("{0}/{1}/{2}",Room.Name,Total,CurrentUser.Id);
            PayGo.Email = model.Email;
            PayGo.EmailModify = 0;
            PayGo.LoginType = 0;
            PayGo.OrderComment = "";
            PayGo.NotifyURL = "http://";
            return View();
        }
    }
}
