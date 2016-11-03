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
        public decimal SellPrice { get; set; }
        public Nullable<decimal> DiscountPrice { get; set; }
        public decimal Bonus { get; set; }
        public int Amount { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Information { get; set; }
        public bool Enabled { get; set; }

        public DateTime Created { get; set; }

        public int Creator { get; set; }
        public DateTime Modified { get; set; }
        public string SessionKey { get; set; }

        public int Person { get; set; }
        public int Beds { get; set; }
        public string BedType { get; set; }
        public string RoomBed { get; set; }

        public int UserId { get; set; }

        public void Create()
        {
            
            //[房型,床型,數量]
            var Now = DateTime.Now;
            db.Room.Add(new Room
            {
                Amount = Amount,
                Bonus = Bonus,
                //BussinessBonus = bussinessbouns,
                //PlatformBonus = platformbouns,
                HotelId = HotelId,
                Created = Now,
                DiscountPrice = DiscountPrice,
                Creator = UserId,
                Enabled = true,
                Information = Information,
                Modified = Now,
                Name = Name,
                SellPrice = SellPrice,
                RoomImage = RoomImages(),
                Beds = Beds,
                BedType = BedType,
                Person = Person,
                RoomBed = RoomBed
            });
            db.SaveChanges();
        }
        public void Edit()
        {
            var PersonBed = new List<int>();
            PersonBed.Add(Person);
            PersonBed.Add(int.Parse(BedType));
            PersonBed.Add(Beds);
            //[房型,床型,數量]
            RoomBed = string.Join(",", PersonBed);
            using (var db = new MyAnythingEntities())
            {
                var result = db.Room.Find(ID);
                result.HotelId = HotelId;
                result.Information = Information;
                result.Modified = DateTime.Now;
                result.Name = Name;              
                result.SellPrice = SellPrice;
                result.DiscountPrice = DiscountPrice;
                result.Amount = Amount;
                result.Bonus = Bonus;
                result.Enabled = Enabled;
                result.Person = Person;
                result.RoomBed = RoomBed;
                result.Beds = Beds;
                result.BedType = BedType;
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
        public decimal Sell { get; set; }
        public decimal Price { get; set; }
        public string Feature { get; set; }

        public string Type { get; set; }

        public string Bed { get; set; }

        public int Quantity { get; set; }
        public List<RoomImage> Images { get; set; }
    }
}