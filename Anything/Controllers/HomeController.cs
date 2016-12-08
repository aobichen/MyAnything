using System.Configuration;
using System.Web.Mvc;
using System.Linq;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using Anything.Helpers;
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

           // var a = new FacilityModel().SelectListItems;

            ViewBag.Facility = new FacilityModel().SelectListItems;
            ViewBag.Scenic = new ScenicModel().SelectListItems;

            var City = new Caches().TWCity;
           
            SelectList selectList = new SelectList(City, "ID", "Name", 0);
            ViewBag.City = selectList;
           
            return View();
        }


        
        
        

        public ActionResult Detail(int id)
        {
            var RoomAmt = new RoomAmt();
            var model = _db.Hotel.Where(o => o.ID == id).Select(o =>
                new HotelDetail
                {
                    ID = o.ID,
                    Address = o.Address,
                    options = o.Facility,
                    Images = o.HotelImage.ToList(),
                    Name = o.Name,
                    Feature = o.Feature,
                    Infomation = o.Information,
                    Tel = o.Tel,
                    Scenics = o.Scenics,
                    City = o.City,
                    Area = o.Area,
                    Facilities = o.Facility,
                   
                }).FirstOrDefault();

            var Facilities = model.Facilities.Split(',').Select(int.Parse).ToList();
            //model.Facilities = _db.ServiceOption.Where(o => Facilities.Contains(o.ID)).Select(p => p.Text).ToList();

            var Date = Session["CheckInDate"] == null ? DateTime.Now.AddDays(1):(DateTime)Session["CheckInDate"];
            var DayOfWeek = Date.DayOfWeek.ToString("d");
            var IsHoliday = false;
            
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
                               //CurrentPrice = RoomAmt.CurrentAmt(r.ID),
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
            foreach(var item in model.Rooms){
                item.CurrentPrice = RoomAmt.CurrentAmt(item.ID);
            }
            ViewBag.NearHotels = _db.Hotel.Where(o => o.City == model.City && o.ID != id).OrderBy(o => Guid.NewGuid()).Take(5).ToList();
            var sce = string.IsNullOrEmpty(model.Scenics) ? new List<int>() : model.Scenics.Split(',').Select(int.Parse).ToList();
            ViewBag.Scenics = _db.Scenic.Where(o => sce.Contains(o.ID)).Select(o => o.Name).ToList(); ;
            return View(model);
        }


        [Authorize]
        public ActionResult Booking(int id)
        {
           
            var Room = _db.Room.Find(id);
            var CheckInDate = Session["CheckInDate"] == null ? DateTime.Now.AddDays(1) : (DateTime)Session["CheckInDate"];
            var CheckOutDate = Session["CheckOutDate"] == null ? CheckInDate.AddDays(2): (DateTime)Session["CheckOutDate"];
            var DateSpans = new TimeSpan(CheckOutDate.Ticks - CheckInDate.Ticks).Days;
            var Dates = new List<DateTime>();
            for (var i = 0; i < DateSpans; i++)
            {
                Dates.Add(CheckInDate.AddDays(i));
            }

            var amt = new RoomAmt().CurrentAmt(id);
           
            var Booking = new BookingModel();
            Booking.UnitPrice = amt;
            Booking.Address = Room.Hotel.Address;
            Booking.BedType = _db.CodeFile.Find(Room.BedType).ItemDescription + "/" +Room.BedAmount;
            Booking.Name = Room.Name;
            Booking.ID = Room.ID;
            Booking.MaxPeople = Room.MaxPerson;
            Booking.Tel = Room.Hotel.Tel;
            Booking.Total = amt;
            Booking.CheckInDate = CheckInDate;
            Booking.CheckOutDate = CheckOutDate;
            Booking.RoomType = _db.CodeFile.Find(Room.RoomType).ItemDescription ;
            var Sum = _db.OrderMaster.Where(o => o.ProductId == Room.ID && (CheckInDate >= o.CheckIn && CheckInDate < o.CheckOut)).Select(o => o.Quantity).DefaultIfEmpty(0).Sum();
            Booking.Quantity = Room.Quantity - Sum;
            var Today = DateTime.Now;
            var MyBonus = _db.MyBonus.Where(o => o.UserID == CurrentUser.Id && o.BonusType == BonusStatusEnum.CanUse.ToString() && o.UseMonth == Today.Month).ToList();
            Booking.Bonus = 0;
            if (MyBonus != null && MyBonus.Count > 0)
            {
                Booking.Bonus = MyBonus.Sum(o => o.Bonus);
            }
            return View(Booking);
        }

        [HttpPost]
        public ActionResult Booking([Bind(Exclude = "Bonus")]BookingModel model)
        {
            if (!ModelState.IsValid)
            {
                
                ViewData.Model = model;
                return View();
            }
          
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
            decimal Total = new RoomAmt().CurrentAmt(Room.ID);
           
            

            var PayGo = new PayGoRequest();
            var Now = DateTime.Now;
            PayGo.MerchantOrderNo = Now.ToString("yyyyMMdd") + Guid.NewGuid().GetHashCode().ToString("x").ToUpper();
            PayGo.LangType = "zh-tw";
            PayGo.TimeStamp = Convert.ToInt32(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            PayGo.Amt = Convert.ToInt16(Total) * model.Quantity;
            //PayGo.Amt = 30;
            PayGo.TradeLimit = 120;
            PayGo.ItemDesc = string.Format("{0}{1}房型，NT.{2}元整/訂購人{3}",Room.Hotel.Name,Room.Name,Total,CurrentUser.Email);
            PayGo.Email = model.info.Email;
            PayGo.EmailModify = 0;
            PayGo.LoginType = 0;
            PayGo.OrderComment = "";           
            PayGo.CheckValue = new Pay2Go().CheckValue(PayGo.Amt, PayGo.MerchantOrderNo, PayGo.TimeStamp);        
            PayGo.RespondType = "JSON";

            var ExpireDate = Now.AddDays(2).ToString("yyyy-MM-dd");
            PayGo.ExpireDate = Now.AddDays(2).ToString("yyyyMMdd");
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

            var PayAmt = PayGo.Amt - model.Bonus;

            var order = new OrderMaster
            {
                Address = model.info.Address,
                Amount = PayGo.Amt,
                PayAmt = PayAmt,
                BonusAmt = model.Bonus,
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
                Name = model.info.Name,
                Status = OrderType.Unpaid.ToString(),
                ExpireDate = PayGo.CREDIT == 1 ? (DateTime?)null : DateTime.Parse(ExpireDate)
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
