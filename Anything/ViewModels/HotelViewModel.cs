using Anything.Helpers;
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
        public List<string> Facility { get; set; }
        public List<string> Scenics { get; set; }
        public bool Enabled { get; set; }

        public bool SaleOff { get; set; }
        public string ImgKey { get; set; }

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
                 Hotel.Enabled = Enabled;
                 Hotel.Feature = Feature;
                 Hotel.HotelImage = HotelImages();
                 Hotel.Information = Information;
                 Hotel.Introduce = Introduce;
                 Hotel.Location = Location;
                 Hotel.Modified = Now;
                 Hotel.Name = Name;
                 Hotel.SaleOff = SaleOff;
                 Hotel.Scenics = (Scenics==null || Scenics.Count <=0) ? string.Empty : string.Join(",", Scenics);
                 Hotel.Facility = string.Join(",", Facility);
                 Hotel.Tel = Tel;
                 Hotel.UserId = UserId;
                 Hotel.WebSite = WebSite;
                 Hotel.Enabled = SaleOff;
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
             Hotel.Scenics = string.Join(",", Scenics);
             Hotel.Facility = string.Join(",", Facility);
             Hotel.Tel = Tel;
             Hotel.UserId = UserId;
             Hotel.Enabled = SaleOff;
             Hotel.WebSite = WebSite;
             db.SaveChanges();
         }


         private List<HotelImage> HotelImages()
         {
            var HotelImage = new List<HotelImage>();
            
            if (!string.IsNullOrEmpty(ImgKey))
            {
                HotelImage = (List<HotelImage>)HttpContext.Current.Session[ImgKey];

                if (ID > 0)
                {
                    var Images = db.HotelImage.Where(o => o.HotelId == ID).Select(p => p.Name).ToList();

                    HotelImage = HotelImage.Where(o => !Images.Contains(o.Name)).ToList();
                }

                var Today = DateTime.Now;

                var UserFolder = System.Configuration.ConfigurationManager.AppSettings["UserFolder"];
                var DirectoryPath = Path.Combine(UserFolder,Today.ToString("yyyyMMdd"));
                var DirectoryServerPath = HttpContext.Current.Server.MapPath(DirectoryPath);
                if (!Directory.Exists(DirectoryServerPath))
                {
                    Directory.CreateDirectory(DirectoryServerPath);
                }

                for (var i = 0; i < HotelImage.Count; i++)
                {
                    var FileName = Guid.NewGuid().GetHashCode().ToString("x");
                    var Extension = Path.GetExtension(HotelImage[i].Name);
                    var WebPath = Path.Combine(DirectoryPath, FileName + Extension);
                    var ServerPath = Path.Combine(DirectoryServerPath, FileName + Extension);
                    MemoryStream ms = new MemoryStream(HotelImage[i].Image);
                    var Image = System.Drawing.Image.FromStream(ms);
                    Image.Save(ServerPath);
                    HotelImage[i].Sort = i + 1;
                    HotelImage[i].Enabled = true;
                    HotelImage[i].Path = WebPath;
                    HotelImage[i].Name = FileName;
                }              
            }
            return HotelImage;
        }
        
    }


    public class HotelDetail
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Feature { get; set; }
        public string Address { get; set; }

        public string Infomation { get; set; }

        public string Tel { get; set; }

        public string Scenics { get; set; }

        public int City { get; set; }

        public int Area { get; set; }
        public string options { get; set; }
        public string Facility { get; set; }
        public List<string> Facilities { get; set; }
        public List<HotelImage> Images { get; set; }

        public List<RoomModel> Rooms { get; set; }
    }

    public class HotelSearchViewModel
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<string> Facility { get; set; }

        public List<string> Scenic { get; set; }

        public string Price { get; set; }

        public int City { get; set; }

        public string Word { get; set; }
    }

    public class HotelsViewModel
    {
        public int ID { get; set; }

        public int RoomID { get; set; }

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

        public string Facility { get; set; }
        public string Scenic { get; set; }

      
         //[Required]
        [DisplayFormat(DataFormatString="{0:#,##0}",ApplyFormatInEditMode=true)]
        public decimal FixedPrice { get; set; }
         //[Required]
        public decimal HolidayPrice { get; set; }
        public decimal DayPrice { get; set; }

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


        public List<HotelsViewModel> GetHotels(HotelSearchViewModel model)
        {

            var Facilities = string.Empty;
            if(model.Facility==null){
                model.Facility = new List<string>();

            }
            else
            {
                Facilities = string.Join(",", model.Facility);
            }

            if (model.Scenic == null)
            {
                model.Scenic = new List<string>();
            }
           

            var _db = new MyAnythingEntities();
            var result = new List<HotelsViewModel>();
            var take = (string.IsNullOrEmpty(model.Price) && model.Facility.Count <=0) ? 30 : 300;

            if (model == null || model.BeginDate == DateTime.MinValue)
            {
                model.BeginDate = DateTime.Now;
            }

            var MinPrice = 0;
            var MaxPrice = 0;

            #region ## 價格區間
            switch (model.Price)
            {
                case "1":
                    MinPrice = 0;
                    MaxPrice = 999;
                    break;
                case "2":
                    MinPrice = 1000;
                    MaxPrice = 1999;
                    break;
                case "3":
                    MinPrice = 2000;
                    MaxPrice = 2999;
                    break;
                case "4":
                    MinPrice = 3000;
                    MaxPrice = 3999;
                    break;
                case "5":
                    MinPrice = 4000;
                    MaxPrice = 4999;
                    break;
                case "6":
                    MinPrice = 5000;
                    MaxPrice = 9999999;
                    break;
            }
            #endregion

            

            result = (from h in _db.Hotel
                          join city in _db.City
                          on h.City equals city.ID
                          join area in _db.Area
                          on h.Area equals area.ID
                          where h.Room.ToList().Count > 0 &&
                          (
                          (string.IsNullOrEmpty(model.Word) || 
                          (h.Name.Contains(model.Word))) &&
                          (model.City <= 0 || h.City ==model.City) 
                          
                          )
                          select new HotelsViewModel
                          {
                              
                              ID =h.ID,
                              Address = h.Address,
                              Location = city.Name + area.Name,
                              Name = h.Name,
                              Feature = h.Feature,
                              Images = h.HotelImage.ToList(),
                              Facility = h.Facility,
                              Scenic = h.Scenics
                              //HolidayPrice = h.Room[0].HolidayPrice,
                              //DayPrice = rp.Room.DayPrice,
                              //FixedPrice = rp.Room.FixedPrice
                              //SellPrice = rp.Price
                          }).Take(take).ToList();
            var RoomAmt = new RoomAmt();
            var r_result = new List<HotelsViewModel>();
            foreach (var m in result)
            {
                var Amts = new List<decimal>();
                var Rooms = _db.Room.Where(o=>o.HotelId == m.ID).ToList();
                foreach (var item in Rooms)
                {
                    Amts.Add(RoomAmt.CurrentAmt(item.ID));
                    
                }

                m.FixedPrice = Amts.Min();

                //if (model.Facility.Count > 0 && model.Scenic.Count > 0)
                //{
                //    var f = m.Facility.Split(',').ToList();
                //    var s = m.Scenic.Split(',').ToList();
                //    if (f.Contains(m.Facility) && s.Contains(m.Scenic))
                //    {
                //        r_result.Add(m);
                //    }
                //}
                //else if (model.Facility.Count > 0)
                //{
                //    var f = m.Facility.Split(',').ToList();
                    
                //    if (f.Contains(m.Facility))
                //    {
                //        r_result.Add(m);
                //    }
                //}

                
                //var price = m
            }

           
            _db.Dispose();

            if(!string.IsNullOrEmpty(model.Price)){
            result = result.Where(o=>o.FixedPrice >= MinPrice && o.FixedPrice <= MaxPrice).ToList();
            }
            return result;
        }
    }

    public class HotelListModel
    {
        public int ID { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }

        public int Qty { get; set; }
        public bool Enabled { get; set; }
    }

    public class RoomSumModel
    {
        public int ID { get; set; }

        public int hotelId { get; set; }
        public int Sum { get; set; }
    }

    

   
    
}