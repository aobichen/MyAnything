using Anything.Helper;
using Anything.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.ViewModels
{
    public class HotelCreateViewModel
    {
        public MyAnythingEntities db;

        public HotelCreateViewModel(){
            if (db == null)
            {
                db = new MyAnythingEntities();
            }
        }
        public int ID { get; set; }
       
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

         [Required]
        public int City { get; set; }

         [Required]
        public int Area { get; set; }

         [Required]
        public string Address { get; set; }
        public string WebSite { get; set; }

         [Required]
        public string Introduce { get; set; }

         [Required]
        public string Feature { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Information { get; set; }
        public string ServiceOptions { get; set; }
        public string Scenics { get; set; }
        public bool Enabled { get; set; }

        public bool SaleOff { get; set; }
        public string SessionKey { get; set; }

        public int UserId { get; set; }

         [Required]
        public string Tel { get; set; }

         public void Create()
         {
             var Now = DateTime.Now;
             
                 var Hotel = new Hotel();
                 Hotel.Address = Address;
                 Hotel.Area = Area;
                 Hotel.City = City;
                 Hotel.Created = Now;
                 Hotel.Enabled = true;
                 Hotel.Feature = Feature;
                 Hotel.HotelImage = HotelImages();
                 Hotel.Information = Information;
                 Hotel.Introduce = Introduce;
                 Hotel.Location = Location;
                 Hotel.Modified = Now;
                 Hotel.Name = Name;
                 Hotel.SaleOff = SaleOff;
                 Hotel.Scenics = Scenics;
                 Hotel.ServiceOptions = ServiceOptions;
                 Hotel.Tel = Tel;
                 Hotel.UserId = UserId;
                 Hotel.WebSite = WebSite;
                 db.Hotel.Add(Hotel);
                 db.SaveChanges();
             
         }

         public void Edit()
         {
             var Hotel = db.Hotel.Find(ID);
             var Now = DateTime.Now;
             Hotel.Address = Address;
             Hotel.Area = Area;
             Hotel.City = City;
             Hotel.Created = Now;
             Hotel.Enabled = Enabled;
             Hotel.Feature = Feature;
             Hotel.HotelImage = HotelImages();
             Hotel.Information = Information;
             Hotel.Introduce = Introduce;
             Hotel.Location = Location;
             Hotel.Modified = Now;
             Hotel.Name = Name;
             Hotel.SaleOff = SaleOff;
             Hotel.Scenics = Scenics;
             Hotel.ServiceOptions = ServiceOptions;
             Hotel.Tel = Tel;
             Hotel.UserId = UserId;
             Hotel.WebSite = WebSite;
             db.SaveChanges();
         }


         private List<HotelImage> HotelImages()
         {
            var HotelImage = new List<HotelImage>();
            
            if (!string.IsNullOrEmpty(SessionKey))
            {
                HotelImage = (List<HotelImage>)HttpContext.Current.Session[SessionKey];

                if (ID > 0)
                {
                    var Images = db.HotelImage.Where(o => o.HotelId == ID).Select(p => p.Name).ToList();

                    HotelImage = HotelImage.Where(o => !Images.Contains(o.Name)).ToList();
                }

                var UserFolder = System.Configuration.ConfigurationManager.AppSettings["UserFolder"];

                for (var i = 0; i < HotelImage.Count; i++)
                {
                    var fileName = Guid.NewGuid().ToString();
                   // var Extension = Path.GetExtension(fileName);
                    var webPath = Path.Combine(UserFolder, fileName + ".jpg");
                    var path = Path.Combine(HttpContext.Current.Server.MapPath(UserFolder), fileName + ".jpg");
                    MemoryStream ms = new MemoryStream(HotelImage[i].Image);
                    var returnImage = System.Drawing.Image.FromStream(ms);
                    HotelImage[i].Sort = i + 1;
                    HotelImage[i].Enabled = true;
                    HotelImage[i].Path = path;
                }              
            }
            return HotelImage;
        }
        
    }


    


    public class HotelsViewModel
    {
        public int ID { get; set; }

         [Required]
        public string Name { get; set; }

         [Required]
        public string Location { get; set; }

         [Required]
        public int City { get; set; }

         [Required]
        public int Area { get; set; }

         [Required]
        public string Address { get; set; }
        public string WebSite { get; set; }
        public string Introduce { get; set; }
        public string Feature { get; set; }

         [Required]
        public decimal SellPrice { get; set; }
         [Required]
        public decimal? DiscountPrice { get; set; }

        public bool HasWifi { get; set; }

        public bool SaleOff { get; set; }


        public string Tel { get; set; }

        public bool HasCarPark { get; set; }

        public List<HotelImage> Images { get; set; }

        public string ServiceOptions { get; set; }
    }

    public class HotelListViewModel
    {
        public List<HotelsViewModel> HotelItems { get; set; }

        public bool HasMore { get; set; }

        public List<int> BeforeItems { get; set; }


        public HotelListViewModel GetHotels(HomeSearchViewModel model=null)
        {

            var _db = new MyAnythingEntities();
            var take = 5;
            var r_result = new HotelListViewModel();
            if (model == null||model.City == 0)
            {
                //取得廣告置入
                
                var model_null_result = (from h in _db.Hotel
                              where h.Enabled == true && h.SaleOff == true && h.Room.Count > 0
                                         select new HotelsViewModel
                              {
                                  Address = h.Address,
                                  Area = h.Area,
                                  City = h.City,
                                  Feature = h.Feature,
                                  ID = h.ID,
                                  SellPrice = h.Room.Count <= 0 ? 0 : h.Room.Min(o => o.SellPrice),
                                  DiscountPrice = h.Room.Count <= 0 ? 0 : h.Room.Min(o => o.DiscountPrice),
                                  Location = h.Location,
                                  Tel = h.Tel,
                                  WebSite = h.WebSite,
                                  Images = h.HotelImage.ToList(),
                                  ServiceOptions = h.ServiceOptions,
                                  Name = h.Name

                              }).OrderBy(r => Guid.NewGuid());

                r_result.HotelItems =  model_null_result == null ? null : model_null_result.Take(take).ToList();
                r_result.HasMore = model_null_result.Count() > r_result.HotelItems.Count?true:false;
                r_result.BeforeItems = (from items in r_result.HotelItems select items.ID).ToList();
                return r_result;
            }
           
            var dbRoom = (  from hotel in _db.Hotel
                            join room in _db.Room on hotel.ID equals room.HotelId
                            where hotel.City == model.City
                            group room by new { room.HotelId, room.ID, room.Amount } into g
                            select new RoomSumModel
                            {
                                hotelId = g.Key.HotelId,
                                ID = g.Key.ID,
                                Sum = g.Sum(o => o.Amount)
                            }).ToList();
                                  

            var HotelId = new List<int>();

            foreach (var room in dbRoom)
            {
                var orders = _db.OrderMaster.Where(o => o.ProductId == room.ID && (o.CheckIn < model.BeginDate && o.CheckOut >= model.EndDate)).ToList();
                var orderAmount = 0;
                if (orders != null && orders.Count > 0)
                {
                    orderAmount = orders.Sum(o => o.Amount);
                }
                if (room.Sum > orderAmount)
                {
                    HotelId.Add(room.ID);
                }
            }

            var result = (from hotel in _db.Hotel
                          where HotelId.Contains(hotel.ID)
                          select new HotelsViewModel
                          {
                             Address = hotel.Address,
                             Area = hotel.Area,
                             City = hotel.City,
                             SellPrice = hotel.Room.Count <= 0 ? 0 : hotel.Room.Min(o => o.SellPrice),
                             DiscountPrice = hotel.Room.Count <= 0 ? 0 : hotel.Room.Min(o => o.DiscountPrice),
                             Feature = hotel.Feature,
                             ID = hotel.ID,
                             Introduce = hotel.Introduce,
                             Images = hotel.HotelImage.ToList(),
                             ServiceOptions = hotel.ServiceOptions

                          }).OrderBy(r => Guid.NewGuid());

            r_result.HotelItems = result.Take(take).ToList(); ;
            r_result.HasMore = result.Count() > r_result.HotelItems.Count ? true : false;
            r_result.BeforeItems = (from items in r_result.HotelItems select items.ID).ToList();
            return r_result;
        }
    }


    public class RoomSumModel
    {
        public int ID { get; set; }

        public int hotelId { get; set; }
        public int Sum { get; set; }
    }

    public class ServiceOptionCheckbox : ServiceOption
    {
        public bool Checked { get; set; }

        public List<ServiceOptionCheckbox> ConverToCheckbox(string[] checkedValue)
        {
            var items = new List<ServiceOption>();
            var checkbox = new List<ServiceOptionCheckbox>();
            using (var db = new MyAnythingEntities())
            {
                items = db.ServiceOption.ToList();
            }

            

            if (items == null || items.Count <= 0)
            {

                return checkbox;
            }

            foreach (var item in items)
            {
                var Id = item.ID.ToString();
                var IsChecked = checkedValue==null ? false : checkedValue.Any(o => o.Contains(Id));
                checkbox.Add(new ServiceOptionCheckbox() { Checked = IsChecked, Text = item.Text, Description = item.Description, ID = item.ID, Enabled = item.Enabled });  
            }
            
            return checkbox;
        }
    }

    public class ScenicsCheckbox : Scenic
    {
        public bool Checked { get; set; }

        public List<ScenicsCheckbox> ConverToCheckbox(string[] checkedValue)
        {
            var items = new List<Scenic>();
            var checkbox = new List<ScenicsCheckbox>();
            using (var db = new MyAnythingEntities())
            {
                items = db.Scenic.ToList();
            }

            if (items == null || items.Count <= 0)
            {

                return checkbox;
            }


           

            foreach (var item in items)
            {
                var Id = item.ID.ToString();
                var IsChecked = checkedValue == null ? false : checkedValue.Any(o => o.Contains(Id));
                checkbox.Add(new ScenicsCheckbox() { Checked = IsChecked, City = item.City, Description = item.Description, Name = item.Name, ID = item.ID, Enabled = item.Enabled });
            }
            
            return checkbox;
        }
    }
    
}