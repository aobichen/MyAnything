﻿using System.Configuration;
using System.Web.Mvc;
using System.Linq;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using Anything.Helper;
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
                           {
                               BedType = code.ItemDescription,
                               Feature = r.Feature,
                               Images = r.RoomImage.ToList(),
                               Name = r.Name,
                               FixedPrice = r.FixedPrice,
                               HolidayPrice = r.HolidayPrice,
                               Quantity = r.Quantity,
                               RoomType = code2.ItemDescription,
                               DayPrice = IsHoliday ? r.HolidayPrice : r.DayPrice,
                               BedAmount = r.BedAmount
                           }).ToList();

            ViewBag.NearHotels = _db.Hotel.Where(o => o.City == model.City && o.ID != id).OrderBy(o => Guid.NewGuid()).Take(5).ToList();
            var sce = model.Scenics.Split(',').Select(int.Parse).ToList();
            ViewBag.Scenics = _db.Scenic.Where(o => sce.Contains(o.ID)).Select(o => o.Name).ToList(); ;
            return View(model);
        }


        [Authorize]
        public ActionResult Booking(int id)
        {
            ViewBag.Message = "Your app description page.";
            var Room = _db.Room.Find(id);
            var CheckInDate = Session["CheckInDate"] == null ? DateTime.Now : (DateTime)Session["CheckInDate"];
            var MaxDate = CheckInDate.AddDays(1);

            var Prices = new List<DatePrices>();

            #region
            for (DateTime date = CheckInDate; MaxDate.CompareTo(CheckInDate) > 0; date.AddDays(1.0))
            {
                if (date.DayOfWeek.ToString() == "5" || date.DayOfWeek.ToString() == "6")
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
                        Prices.Add(new DatePrices { Date = date.ToShortDateString(), Price = price.ToString("#,##0") });
                    }
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
                        Prices.Add(new DatePrices { Date = date.ToShortDateString(), Price = price.ToString("#,##0") });
                    }
                }
            }
            ViewBag.PriceList = Prices;
            #endregion

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
