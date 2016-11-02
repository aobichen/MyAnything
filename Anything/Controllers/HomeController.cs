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


            //var headad = _db.AdOrder.Where(o => o.Position == "首頁看板").ToList();
            
            //var hasImage = new List<AdvImage>();
            //foreach (var had in headad)
            //{
            //    foreach(var img in had.AdImage){
            //        var image = new AdvImage();
            //        image.ID = img.ID;
            //        image.Image = img.Image;
            //        image.Text = had.Text;
            //        image.Url = had.Href;
            //        hasImage.Add(image);
            //    }
            //}

           // ViewBag.HeadAd = hasImage;
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
            var model = _db.Hotel.Find(id);
            var key = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.SessionKey = key;

            var RoomSelectedAmount = new List<SelectedAmount>();
            if (model !=null && model.Room !=null && model.Room.Count > 0)
            {
                foreach (var r in model.Room)
                {
                    var Order = _db.OrderMaster.GroupBy(o => o.ProductId == r.ID).Select(g => new { id = g.Key, total = g.Sum(i => i.Amount) }).FirstOrDefault();
                    var Order_Amount = 0;
                    if (Order != null)
                    {
                        Order_Amount = Order.total;
                    }
                    r.Amount = r.Amount - Order_Amount <= 0 ? 0 : r.Amount - Order_Amount;
                    RoomSelectedAmount.Add(new SelectedAmount { Amount = 1, RoomId = r.ID });
                }
            }
            Session[key] = RoomSelectedAmount;
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
