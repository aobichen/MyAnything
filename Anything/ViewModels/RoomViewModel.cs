﻿using Anything.Models;
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
        [Required]
        public int HotelId { get; set; }
         [Required]
        public string Name { get; set; }

         [Required]
        [DisplayFormat(DataFormatString="{0:#.##}", ApplyFormatInEditMode=true)]
        public decimal FixedPrice { get; set; }

         [Required]
         [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public decimal HolidayPrice { get; set; }

         [Required]
         [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public decimal DayPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Notice { get; set; }
        public bool Enabled { get; set; }

        
        public DateTime Created { get; set; }

        public int Creator { get; set; }
        public DateTime Modified { get; set; }
        //public string SessionKey { get; set; }

         [Required]
         [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public int MaxPerson { get; set; }
         [Required]
        public int BedAmount { get; set; }
        public int BedType { get; set; }
        public int RoomType { get; set; }
        public string RoomBed { get; set; }

        [Required]
        public string Feature { get; set; }
        public int UserId { get; set; }

        public string ImgKey { get; set; }

        public bool HasBreakfast { get; set; }
        public void Create()
        {
            
            //[房型,床型,數量]
            var Now = DateTime.Now;
            var result = new Room();
            result.HotelId = HotelId;
            result.Notice = Notice;
            result.Created = Now;
            result.Modified = Now;
            result.Creator = Creator;
            result.Name = Name;
            result.FixedPrice = FixedPrice;
            result.HolidayPrice = HolidayPrice;
            result.DayPrice = DayPrice;
            result.Quantity = Quantity;
            result.MaxPerson = MaxPerson;
            result.Enabled = Enabled;
            result.RoomType = RoomType;
            //result.RoomBed = RoomBed;
            result.BedAmount = BedAmount;
            result.BedType = BedType;
            result.Feature = string.IsNullOrEmpty(Feature) ? string.Empty : Feature;
            result.HasBreakfast = HasBreakfast;
            result.Modify = Creator;
            //result.RoomBed = string.Empty;
            result.RoomImage = RoomImages();
            db.Room.Add(result);
            db.SaveChanges();
        }
        public void Edit()
        {
            var PersonBed = new List<int>();
           
            RoomBed = string.Join(",", PersonBed);
            using (var db = new MyAnythingEntities())
            {
                var result = db.Room.Find(ID);
                result.HotelId = HotelId;
                result.Notice = Notice;
                result.Modified = DateTime.Now;
                result.Modify = Creator;
                result.Name = Name;
                result.FixedPrice = FixedPrice;
                result.HolidayPrice = HolidayPrice;
                result.DayPrice = DayPrice;
                result.Quantity = Quantity;
                result.MaxPerson = MaxPerson;
                result.Enabled = Enabled;
                result.RoomType = RoomType;               
                result.BedAmount = BedAmount;
                result.BedType = BedType;
                result.Feature = string.IsNullOrEmpty(Feature) ? string.Empty :Feature;
                result.HasBreakfast = HasBreakfast;
                
               
                result.ID = ID;
                
                //db.SaveChanges();

                var RoomImage = RoomImages();
                foreach (var img in RoomImage)
                {
                    img.RoomId = result.ID;
                }
                db.RoomImage.AddRange(RoomImage);
                db.SaveChanges();
            }
        }


        private List<RoomImage> RoomImages()
        {
            var RoomImage = new List<RoomImage>();

            if (!string.IsNullOrEmpty(ImgKey))
            {
                RoomImage = (List<RoomImage>)HttpContext.Current.Session[ImgKey];

                if (ID > 0)
                {
                    var Images = db.RoomImage.Where(o => o.RoomId == ID).Select(p => p.Name).ToList();

                    RoomImage = RoomImage.Where(o => !Images.Contains(o.Name)).ToList();
                }
                if (RoomImage==null || RoomImage.Count <= 0) {
                    return new List<RoomImage>();
                }
                var Today = DateTime.Now;
                var UserFolder = System.Configuration.ConfigurationManager.AppSettings["UserFolder"];
                var DirectoryPath = Path.Combine(UserFolder, Today.ToString("yyyyMMdd"));
                var DirectoryServerPath = HttpContext.Current.Server.MapPath(DirectoryPath);
                if (!Directory.Exists(DirectoryServerPath))
                {
                    Directory.CreateDirectory(DirectoryServerPath);
                }

                for (var i = 0; i < RoomImage.Count; i++)
                {
                    var FileName = Guid.NewGuid().GetHashCode().ToString("x");
                    var Extension = Path.GetExtension(RoomImage[i].Name);
                    var WebPath = Path.Combine(DirectoryPath, FileName + Extension);
                    var ServerPath = Path.Combine(DirectoryServerPath, FileName + Extension);
                    MemoryStream ms = new MemoryStream(RoomImage[i].Image);
                    var Image = System.Drawing.Image.FromStream(ms);
                    Image.Save(ServerPath);
                    RoomImage[i].Sort = i + 1;
                    RoomImage[i].Enabled = true;
                    RoomImage[i].Path = WebPath;
                    RoomImage[i].Name = FileName;
                    RoomImage[i].Extension = Extension;
                    
                }       
            }

            //HttpContext.Current.Session.Remove(ImgKey);
            return RoomImage;
        }
    }

    public class RoomModel
    {
        public int ID { get; set; }
        public int Amt { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public decimal FixedPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal HolidayPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal DayPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public decimal CurrentPrice { get; set; }

        public string Feature { get; set; }

        public bool Enabled { get; set; }

        public string RoomType { get; set; }

        public string BedType { get; set; }

        public bool HasBreakfast { get; set; }

        public int BedAmount { get; set; }
        public int Quantity { get; set; }
        public List<RoomImage> Images { get; set; }
    }

    public class RoomPageModel
    {
        public int ID { get; set; }
        public int Page { get; set; }
    }
}