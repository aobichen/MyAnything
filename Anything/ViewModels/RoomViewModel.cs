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
    public class RoomCreateViewModel
    {
        public MyAnythingEntities db;

        public RoomCreateViewModel()
        {
            if (db == null)
            {
                db = new MyAnythingEntities();
            }
        }

        public int ID { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public decimal FixedPrice { get; set; }
        public decimal HolidayPrice { get; set; }
        public decimal DayPrice { get; set; }

       
        public int Quantity { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Notice { get; set; }
        public bool Enabled { get; set; }

        public DateTime Created { get; set; }

        public int Creator { get; set; }
        public DateTime Modified { get; set; }
        public string SessionKey { get; set; }

        public int MaxPerson { get; set; }
        public int BedAmount { get; set; }
        public int BedType { get; set; }
        public int RoomType { get; set; }
        public string RoomBed { get; set; }

        public string Feature { get; set; }
        public int UserId { get; set; }

        public bool HasBreakfast { get; set; }
        public void Create()
        {
            
            //[房型,床型,數量]
            var Now = DateTime.Now;
            db.Room.Add(new Room
            {
                Quantity = Quantity,              
                HotelId = HotelId,
                Created = Now,
                HolidayPrice = HolidayPrice,
                FixedPrice = FixedPrice,
                DayPrice = DayPrice,
                Creator = UserId,
                Enabled = true,
                Notice = Notice,
                Modified = Now,
                Name = Name,
                BedType=BedType,
                RoomImage = RoomImages(),
                BedAmount = BedAmount,
                HasBreakfast = HasBreakfast,
                MaxPerson = MaxPerson,
                RoomBed = RoomBed,
                RoomType = RoomType,
                Feature = Feature
            });
            db.SaveChanges();
        }
        public void Edit()
        {
            var PersonBed = new List<int>();
            //PersonBed.Add(Person);
            //PersonBed.Add(int.Parse(BedType));
            //PersonBed.Add(Beds);
            //[房型,床型,數量]
            RoomBed = string.Join(",", PersonBed);
            using (var db = new MyAnythingEntities())
            {
                var result = db.Room.Find(ID);
                result.HotelId = HotelId;
                result.Notice = Notice;
                result.Modified = DateTime.Now;
                result.Name = Name;
                result.FixedPrice = FixedPrice;
                result.HolidayPrice = HolidayPrice;
                result.DayPrice = DayPrice;
                result.Quantity = Quantity;
                result.MaxPerson = MaxPerson;
                result.Enabled = Enabled;
                result.RoomType = RoomType;
                result.RoomBed = RoomBed;
                result.BedAmount = BedAmount;
                result.BedType = BedType;
                result.Feature = Feature;
                result.RoomImage = RoomImages();
                db.SaveChanges();
            }
        }


        private List<RoomImage> RoomImages()
        {
            var RoomImage = new List<RoomImage>();

            if (!string.IsNullOrEmpty(SessionKey))
            {
                RoomImage = (List<RoomImage>)HttpContext.Current.Session[SessionKey];

                if (ID > 0)
                {
                    var Images = db.RoomImage.Where(o => o.RoomId == ID).Select(p => p.Name).ToList();

                    RoomImage = RoomImage.Where(o => !Images.Contains(o.Name)).ToList();
                }

                var UserFolder = System.Configuration.ConfigurationManager.AppSettings["UserFolder"];

                for (var i = 0; i < RoomImage.Count; i++)
                {
                    var fileName = Guid.NewGuid().ToString();
                    // var Extension = Path.GetExtension(fileName);
                    var webPath = Path.Combine(UserFolder, fileName + ".jpg");
                    var path = Path.Combine(HttpContext.Current.Server.MapPath(UserFolder), fileName + ".jpg");
                    MemoryStream ms = new MemoryStream(RoomImage[i].Image);
                    var returnImage = System.Drawing.Image.FromStream(ms);
                    RoomImage[i].Sort = i + 1;
                    RoomImage[i].Enabled = true;
                    RoomImage[i].Path = path;
                }
            }
            return RoomImage;
        }
    }

    public class RoomModel
    {
        public string Name { get; set; }
        public decimal FixedPrice { get; set; }
        public decimal HolidayPrice { get; set; }
        public decimal DayPrice { get; set; }
        public string Feature { get; set; }

        public string RoomType { get; set; }

        public string BedType { get; set; }

        public bool HasBreakfast { get; set; }

        public int BedAmount { get; set; }
        public int Quantity { get; set; }
        public List<RoomImage> Images { get; set; }
    }
}