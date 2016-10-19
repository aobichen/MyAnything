using Anything.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Anything.WebApi
{
    public class imagedelete
    {
        public string name { get; set; }
        public string key { get; set; }
    }
    public class ImageUploadController : ApiController
    {
        private AnythingEntities db;
        public ImageUploadController()
            : base()
        {
            db = new AnythingEntities();
            UserFolder = System.Configuration.ConfigurationManager.AppSettings["UserFolder"];
        }

        private int Limit = 20;

        private string UserFolder { get; set; }

        [HttpPost]
        [Route("AdImageDelete")]
        public object AdImageDelete(imagedelete data)
        {
            var ImageFile = db.AdImage.Where(o => o.Name == data.name).FirstOrDefault();
            if (ImageFile != null)
            {
                db.AdImage.Attach(ImageFile);
                db.AdImage.Remove(ImageFile);
                db.SaveChanges();
            }

            var path = Path.Combine(HttpContext.Current.Server.MapPath(UserFolder), data.name + ".jpg");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            if (HttpContext.Current.Session[data.key] != null)
            {
                var images = (List<HotelImage>)HttpContext.Current.Session[data.key];
                if (images.Count > 0)
                {
                    images.Remove(images.Where(o => o.Name == data.name).FirstOrDefault());
                }

                HttpContext.Current.Session[data.key] = images;
            }

            return Json(new { success = true });
        }

        [HttpPost]
        [Route("HotelImageDelete")]
        public object HotelImageDelete(imagedelete data)
        {
            var ImageFile = db.HotelImage.Where(o => o.Name == data.name).FirstOrDefault();
            if (ImageFile!=null)
            {
                db.HotelImage.Attach(ImageFile);
                db.HotelImage.Remove(ImageFile);
                db.SaveChanges();
            }

            var path = Path.Combine(HttpContext.Current.Server.MapPath(UserFolder),data.name+".jpg");

            if(File.Exists(path)){
                File.Delete(path);
            }

            if (HttpContext.Current.Session[data.key] != null)
            {
                var images = (List<HotelImage>)HttpContext.Current.Session[data.key];
                if (images.Count > 0)
                {
                    images.Remove(images.Where(o=>o.Name == data.name).FirstOrDefault());
                }

                HttpContext.Current.Session[data.key] = images;
            }

            return Json(new { success = true });
        }

        [HttpPost]
        [Route("HotelImageUpload")]
        public object HotelImageUpload()
        {
            var Current = HttpContext.Current;
            var key = HttpContext.Current.Request["key"];
            var FolderPath = UserFolder;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(FolderPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(FolderPath));
            }

            var Images = new List<HotelImage>();

            if (HttpContext.Current.Session[key] != null)
            {

                Images = (List<HotelImage>)Current.Session[key];
                Images = Images.Select(o => { o.Hotel = null; return o; }).ToList();
            }

            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                var file = HttpContext.Current.Request.Files[i];
               // var image = FileToByte(file);
                var subName = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid().ToString();
                var webPath = Path.Combine(FolderPath, fileName + ".jpg");
                var path = Path.Combine(HttpContext.Current.Server.MapPath(FolderPath),fileName+".jpg");
                file.SaveAs(path);
                var image = FileToByte(file);
                Images.Add(new HotelImage { Image = image, Name = fileName, Path = webPath, Sort = i+1 });
            }

            var Message = "完成上傳";

            if (Images.Count >= Limit)
            {
                Message = string.Format("照片限制{0}張",Limit);
                Images = Images.Take(Limit).ToList();
            }
            Current.Session[key] = Images;
            var data = JsonConvert.SerializeObject(Images);
            return Json(new { data = data, message = Message });
        }

        [HttpPost]
        [Route("RoomImageDelete")]
        public object RoomImageDelete(imagedelete data)
        {
            var ImageFile = db.RoomImage.Where(o => o.Name == data.name).FirstOrDefault();
            if (ImageFile != null)
            {
                db.RoomImage.Attach(ImageFile);
                db.RoomImage.Remove(ImageFile);
                db.SaveChanges();
            }

            var path = Path.Combine(HttpContext.Current.Server.MapPath(UserFolder), data.name + ".jpg");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            if (HttpContext.Current.Session[data.key] != null)
            {
                var images = (List<RoomImage>)HttpContext.Current.Session[data.key];
                if (images.Count > 0)
                {
                    images.Remove(images.Where(o => o.Name == data.name).FirstOrDefault());
                }

                HttpContext.Current.Session[data.key] = images;
            }

            return Json(new { success = true });
        }

        [HttpPost]
        [Route("RoomImageUpload")]
        public object RoomImageUpload()
        {
            var Current = HttpContext.Current;
            var key = HttpContext.Current.Request["key"];
            var FolderPath = UserFolder;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(FolderPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(FolderPath));
            }

            var Images = new List<RoomImage>();

            if (HttpContext.Current.Session[key] != null)
            {

                Images = (List<RoomImage>)Current.Session[key];
                Images = Images.Select(o => { o.Room = null; return o; }).ToList();
            }

            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                var file = HttpContext.Current.Request.Files[i];
                // var image = FileToByte(file);
                var subName = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid().ToString();
                var webPath = Path.Combine(FolderPath, fileName + ".jpeg");
                var path = Path.Combine(HttpContext.Current.Server.MapPath(FolderPath), fileName + ".jpeg");
                file.SaveAs(path);
                var image = FileToByte(file);
                Images.Add(new RoomImage { Image = image, Name = fileName, Path = webPath, Sort = i + 1 });
            }

            var Message = "完成上傳";

            if (Images.Count >= Limit)
            {
                Message = string.Format("照片限制{0}張", Limit);
                Images = Images.Take(Limit).ToList();
            }
            Current.Session[key] = Images;
            var data = JsonConvert.SerializeObject(Images);
            return Json(new { data = data, message = Message });
        }

        private byte[] FileToByte(HttpPostedFile file){
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }
            return fileData;
        }
    }
}
