using System.Configuration;
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
                    Infomation = o.Information
                }).FirstOrDefault();

            var Facilities = model.options.Split(',').Select(int.Parse).ToList();
            model.Facilities = _db.ServiceOption.Where(o => Facilities.Contains(o.ID)).Select(p => p.Text).ToList();
            model.Rooms = (from r in _db.Room
                           join code in _db.CodeFile
                               on r.Beds equals code.ID
                               where r.HotelId == model.ID
                           select new RoomModel
                           {
                               Bed = code.ItemDescription,
                               Feature = r.Feature,
                               Images = r.RoomImage.ToList(),
                               Name = r.Name,
                               Price = r.DiscountPrice.Value,
                               Sell = r.SellPrice,
                               Quantity = r.Amount,
                               Type = r.Person.ToString()
                           }).ToList();
           
            return View(model);
        }


        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
