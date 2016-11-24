using System.Configuration;
using System.Web.Mvc;
using System.Linq;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using Anything.Helper;
using Anything.Models;
using System.Data.Entity;
namespace Anything.Controllers
{
    public class HomeController : BaseController
    {
        
    
        public ActionResult Index(HomeSearchViewModel model = null,string username = null)
        {
            var Now = DateTime.Now;
            Session["CheckInDate"] = Now.AddDays(1);
            Session["CheckOutDate"] = Now.AddDays(2);
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

            var Date = Session["CheckInDate"] == null ? DateTime.Now.AddDays(1):(DateTime)Session["CheckInDate"];
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
                                ( 
                                
                                 (DbFunctions.DiffDays(o.CheckIn, Date) == 0) ||
                                 (DbFunctions.DiffDays(o.CheckOut, Date) == 0)

                                 )).Select(o=>o.Quantity).DefaultIfEmpty(0).Sum()
                           }).ToList();

            model.Rooms = model.Rooms.Where(o => o.Quantity >= o.Amt).ToList();

            ViewBag.NearHotels = _db.Hotel.Where(o => o.City == model.City && o.ID != id).OrderBy(o => Guid.NewGuid()).Take(5).ToList();
            var sce = model.Scenics.Split(',').Select(int.Parse).ToList();
            ViewBag.Scenics = _db.Scenic.Where(o => sce.Contains(o.ID)).Select(o => o.Name).ToList(); ;
            return View(model);
        }


        [Authorize]
        public ActionResult Booking(int id)
        {
           
            var Room = _db.Room.Find(id);
            var CheckInDate = Session["CheckInDate"] == null ? DateTime.Now.AddDays(1) : (DateTime)Session["CheckInDate"];
            var CheckOutDate = Session["CheckOutDate"] == null ? CheckInDate.AddDays(1): (DateTime)Session["CheckOutDate"];
            var DateSpans = new TimeSpan(CheckOutDate.Ticks - CheckInDate.Ticks).Days;
            var Dates = new List<DateTime>();
            for (var i = 0; i < DateSpans; i++)
            {
                Dates.Add(CheckInDate.AddDays(i));
            }

            //var MaxDate = CheckInDate.AddDays(10);
            var MaxDate = CheckOutDate;
            var Prices = new List<DatePrices>();
            decimal Total = 0;
            decimal UnitPrice = 0;
            #region
            for (DateTime date = CheckInDate; MaxDate.CompareTo(date) > 0; date = date.AddDays(1.0))
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

                    var Checked = Dates.Where(o => o.Year == date.Year && o.Month == date.Month && o.Date == date.Date).Any();

                    if (Checked)
                    {
                        Total += price;
                    }
                    UnitPrice = price;
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

                    UnitPrice = price;
                    Prices.Add(new DatePrices { Date = date.ToShortDateString(), Price = price.ToString("#,##0"), Checked = Checked });
                }
            }
            //ViewBag.PriceList = Prices;
            #endregion

            var Booking = new BookingModel();
            Booking.UnitPrice = UnitPrice;
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
            var Sum = _db.OrderMaster.Where(o => o.ProductId == Room.ID && (CheckInDate >= o.CheckIn && CheckInDate < o.CheckOut)).Select(o => o.Quantity).DefaultIfEmpty(0).Sum();
            Booking.Quantity = Room.Quantity - Sum;
            return View(Booking);
        }

        [HttpPost]
        public ActionResult Booking(BookingModel model)
        {
            var Room = _db.Room.Find(model.ID);
            //var DateTims = model.DateList.Split(',');
            model.CheckInDate = Session["CheckInDate"] == null ? DateTime.Now.AddDays(1) : (DateTime)Session["CheckInDate"];
            model.CheckOutDate = Session["CheckOutDate"] == null ? model.CheckInDate.AddDays(1) : (DateTime)Session["CheckOutDate"];
            var Sum = _db.OrderMaster.Where(o => o.ProductId == Room.ID &&
                                (
                                 (DbFunctions.DiffDays(o.CheckIn, model.CheckInDate) == 0) ||
                                 (DbFunctions.DiffDays(o.CheckOut, model.CheckOutDate) == 0)
                                 )).Select(o => o.Quantity).DefaultIfEmpty(0).Sum();
            var Filled = Room.Quantity >= Sum;
            if (!Filled)
            {
                ModelState.AddModelError("","客滿");
                return RedirectToAction("Detail", new { id=Room.Hotel.ID });
            }
            //var Dates = model.DateList.Split(',').Select(DateTime.Parse).ToList();
            var CheckInDate = model.CheckInDate;
            var CheckOutDate = model.CheckOutDate;
            decimal Total = 0;
           
            #region ## 檢查 ##
            for (DateTime date = CheckInDate; CheckOutDate.CompareTo(date) > 0; date = date.AddDays(1.0))
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
            var Now = DateTime.Now;
            PayGo.MerchantOrderNo = Now.ToString("yyyyMMdd") + Guid.NewGuid().GetHashCode().ToString("x").ToUpper();
            PayGo.LangType = "zh-tw";
            PayGo.TimeStamp = Convert.ToInt32(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            PayGo.Amt = Convert.ToInt16(Total) * model.Quantity;
            //PayGo.Amt = 30;
            PayGo.TradeLimit = 120;
            PayGo.ItemDesc = string.Format("{0}/{1}/{2}",Room.Name,Total,CurrentUser.Id);
            PayGo.Email = model.info.Email;
            PayGo.EmailModify = 0;
            PayGo.LoginType = 0;
            PayGo.OrderComment = "";           
            PayGo.CheckValue = new Pay2Go().CheckValue(PayGo.Amt, PayGo.MerchantOrderNo, PayGo.TimeStamp);        
            PayGo.RespondType = "JSON";
            var ExpireDateTimeSpan = new TimeSpan(CheckInDate.Ticks - Now.Ticks).Days;
            var ExpireTimeSpan = new TimeSpan(CheckInDate.Ticks - Now.Ticks).Hours;
            

            PayGo.ExpireDate = Now.AddDays(1).ToString("yyyyMMdd");
            PayGo.ExpireTime = "";
            var PaymentType = string.Empty;

            #region ##付款方式
            switch (model.PaymentType)
            {
                case 1:                   
                    PayGo.CREDIT = 1;
                    PayGo.InstFlag = "0";
                    PayGo.WEBATM = 0;
                    PayGo.VACC = 0;
                    PayGo.CVS = 0;
                    PaymentType = "信用卡一次付清";
                    break;
                case 2:
                     PayGo.CREDIT = 0;
                    PayGo.InstFlag = "3";
                    PayGo.WEBATM = 0;
                    PayGo.VACC = 0;
                    PayGo.CVS = 0;
                    PaymentType = "信用卡分三期";
                    break;
                case 3:
                     PayGo.CREDIT = 0;
                    PayGo.InstFlag = "0";
                    PayGo.WEBATM = 1;
                    PayGo.VACC = 0;
                    PayGo.CVS = 0;
                    PaymentType = "網路ATM";
                    break;
                case 4:
                    PayGo.CREDIT = 0;
                    PayGo.InstFlag = "0";
                    PayGo.WEBATM = 0;
                    PayGo.VACC = 1;
                    PayGo.CVS = 0;
                     PaymentType = "實體ATM";
                    break;
                case 5:
                    PayGo.CREDIT = 0;
                    PayGo.InstFlag = "0";
                    PayGo.WEBATM = 0;
                    PayGo.VACC = 0;
                    PayGo.CVS = 1;
                    PaymentType = "超商付款";
                    break;
                case 6:
                    PayGo.CREDIT = 0;
                    PayGo.InstFlag = "0";
                    PayGo.WEBATM = 0;
                    PayGo.VACC = 0;
                    PayGo.CVS = 0;
                    PayGo.BARCODE = 1;
                    PaymentType = "條碼付款";
                    break;
            }
            #endregion

            BookingCommit BookCommit = new BookingCommit();
            BookCommit.Booking = model;
            BookCommit.PayGoRequest = PayGo;
            var order = new OrderMaster
            {
                Address = model.info.Address,
                Amount = PayGo.Amt,
                CheckIn = model.CheckInDate,
                CheckOut = model.CheckOutDate,
                MerchantOrderNo = PayGo.MerchantOrderNo,
                Created = DateTime.Now,
                PayVendor = "Pay2Go",
                PaymentType = PaymentType,
                ProductId = Room.ID,
                ProductName = Room.Name,
                ProductType = "Room",
                Creator = CurrentUser.Id,
                Modified = Now,
                Modify = CurrentUser.Id,
                Quantity = model.Quantity,
                Tel = model.info.Phone,
                UserId = CurrentUser.Id,
                Email = model.info.Email,
                Name = model.info.Name
            };
            _db.OrderMaster.Add(order);
            _db.SaveChanges();
            return View("PayCommit", BookCommit);
        }

        public ActionResult PayCommit(PayGoRequest model)
        {
            return View();
        }
    }
}
