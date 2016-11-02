using Anything.Helper;
using Anything.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    [Authorize(Roles = "Admin,AdManager,Hotel")]
    public class AdvertisingController : BaseController
    {
        // GET: Advertising
        public ActionResult Index()
        {
           
            var Advertising = _db.AdManage.Where(o => o.SaleOff == false).ToList();
            ViewBag.Advertising = Advertising;
            return View();
        }

        [Authorize(Roles = "Admin,AdManager")]
        public ActionResult Create(int id)
        {
            var Advertising = _db.AdManage.Where(o => o.ID == id && o.SaleOff == false).FirstOrDefault();
            //var OrderAmount = 0;
            var Now = DateTime.Parse(DateTime.Now.ToShortDateString());
            //if (Advertising == null)
            //{
            //    return RedirectToAction("Index");
            //}

            //OrderAmount = _db.AdOrder.Where(o => o.AdId == id && Now > o.EndDate).Count();

            //if (OrderAmount == Advertising.MaxCount)
            //{
            //    ModelState.AddModelError("", "廣告已售完");
            //    return View();
            //}

            var model = new AdOrder();
            model.AdId = Advertising.ID;
            model.AllowImage = Advertising.AllowImage;
            model.AllowText = Advertising.AllowText;
            model.ImageHeight = Advertising.ImageHeight;
            model.ImageWidth = Advertising.ImageWidth;
            model.ImageLimit = Advertising.ImageLimit;
            model.TextLimit = Advertising.TextLimit;
            model.BeginDate = Now.AddDays(1);
            model.EndDate = Now.AddDays((Advertising.Days + 1));
            model.Position = Advertising.Position;
            model.PositionId = Advertising.PositionId;
            model.Days = Advertising.Days;
            model.bought = 0;
            var SessionKey = Guid.NewGuid().GetHashCode().ToString("x");
            Session[SessionKey] = Advertising;
            ViewBag.SessionKey = SessionKey;
            return View(model);
        }


        [HttpPost]
        public ActionResult Create(AdOrder model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (User.IsInRole("Admin") || User.IsInRole("AdManager"))
            {
                model.Status = "official";
                model.Name = "System";
            }


            var SessionKey = string.Empty;
            var Ad = new AdManage();
            if (Request["key"] != null)
            {
                SessionKey = Request["key"];
                Ad = (AdManage)Session[SessionKey];
            }

            //var AdId = Ad.ID;
            //var Orders = _db.AdOrder.Where(o => o.AdId == AdId).Count();
            //if (Orders >= Ad.MaxCount)
            //{
            //    return RedirectToAction("Index");
            //}

            #region 處理圖片
            var Images = new List<AdImage>();

            if (Request.Files.Count > 0)
            {
                var Files = Request.Files;
                var UserFolder = System.Configuration.ConfigurationManager.AppSettings["UserFolder"];

                if (!Directory.Exists(Server.MapPath(UserFolder)))
                {
                    Directory.CreateDirectory(Server.MapPath(UserFolder));
                }

                for (var i = 0; i < Files.Count; i++)
                {
                    if (i >= Ad.ImageLimit)
                    {
                        break;
                    }
                    var file = Request.Files[i];
                    var Name = Guid.NewGuid().ToString();
                    var path = string.Format("~{0}{1}.jpeg", UserFolder, Name);
                    file.SaveAs(Server.MapPath(path));
                    var img = new AdImage();
                    img.Path = path;
                    img.Name = Name;
                    img.Enabled = true;
                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        img.Image = fileData;
                        Images.Add(img);
                    }
                }
            }
            #endregion
            model.AdId = model.AdId;
            model.AdImage = Images;
            model.Days = Ad.Days;
            model.AllowImage = Ad.AllowImage;
            model.AllowText = Ad.AllowText;
            model.ImageHeight = Ad.ImageHeight;
            model.ImageWidth = Ad.ImageWidth;
            model.ImageLimit = Ad.ImageLimit;
            model.TextLimit = Ad.TextLimit;
            model.Creator = CurrentUser.Id;
            model.Created = DateTime.Now;
            _db.AdOrder.Add(model);
            _db.SaveChanges();

            return RedirectToAction("List");
        }

         [Authorize]
        public ActionResult List()
        {
            if (User.IsInRole("Admin") || User.IsInRole("AdManager"))
            {
                
                //var Status = "official";
                //var Advertising = _db.AdOrder.Where(o => o.Status == Status).ToList();
                var Advertising = _db.AdOrder.ToList();
                ViewBag.Advertising = Advertising;
                return View(Advertising);
            }
            else
            {
                var Advertising = _db.AdOrder.Where(o => o.Creator == CurrentUser.Id).ToList();
                ViewBag.Advertising = Advertising;
                return View(Advertising);
            }
           
        }

        [HttpPost]
        public ActionResult Order(AdOrder model)
        {
           
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (User.IsInRole("Admin") || User.IsInRole("AdManager"))
            {
                model.Status = "official";
            }
            

            var SessionKey = string.Empty;
            var Ad = new AdManage();
            if (Request["key"] != null)
            {
                SessionKey = Request["key"];
                Ad = (AdManage)Session[SessionKey];
            }

            var AdId = Ad.ID;
            var Orders = _db.AdOrder.Where(o => o.AdId == AdId).Count();
            if (Orders >= Ad.MaxCount)
            {
                return RedirectToAction("Index");
            }

            #region 處理圖片
            var Images = new List<AdImage>();

            if (Request.Files.Count > 0)
            {
                var Files = Request.Files;
                var UserFolder = System.Configuration.ConfigurationManager.AppSettings["UserFolder"];

                if (!Directory.Exists(Server.MapPath(UserFolder)))
                {
                    Directory.CreateDirectory(Server.MapPath(UserFolder));
                }
                
                for (var i = 0; i < Files.Count; i++)
                {
                    if (i >= Ad.ImageLimit)
                    {
                        break;
                    }
                    var file = Request.Files[i];
                    var Name = Guid.NewGuid().ToString() ;
                    var path = string.Format("~{0}{1}.jpeg", UserFolder, Name);
                    file.SaveAs(Server.MapPath(path));
                    var img = new AdImage();
                    img.Path = path;
                    img.Name = Name;
                    img.Enabled = true;
                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        img.Image = fileData;
                        Images.Add(img);
                    }
                }
            }
             #endregion

            model.AdImage = Images;
            model.Days = Ad.Days;
            model.AllowImage = Ad.AllowImage;
            model.AllowText = Ad.AllowText;
            model.ImageHeight = Ad.ImageHeight;
            model.ImageWidth = Ad.ImageWidth;
            model.ImageLimit = Ad.ImageLimit;
            model.TextLimit = Ad.TextLimit;
            model.Creator = CurrentUser.Id;
            model.Created = DateTime.Now;
            _db.AdOrder.Add(model);
            _db.SaveChanges();

            return RedirectToAction("List");
        }
        public ActionResult Order(int id)
        {
            _AllPay p = new _AllPay();
            ViewBag.apppay = p.HtmlCode;
            var Advertising = _db.AdManage.Where(o => o.ID == id && o.SaleOff == false).FirstOrDefault();
            var OrderAmount = 0;
            var Now = DateTime.Parse(DateTime.Now.ToShortDateString());
            if (Advertising == null)
            {
                return RedirectToAction("Index");
            }

            OrderAmount = _db.AdOrder.Where(o => o.AdId == id && Now > o.EndDate).Count();

            if (OrderAmount == Advertising.MaxCount)
            {
                ModelState.AddModelError("","廣告已售完");
                return View();
            }

            var model = new AdOrder();
            model.AdId = Advertising.ID;
            model.AllowImage = Advertising.AllowImage;
            model.AllowText = Advertising.AllowText;
            model.ImageHeight = Advertising.ImageHeight;
            model.ImageWidth = Advertising.ImageWidth;
            model.ImageLimit = Advertising.ImageLimit;
            model.TextLimit = Advertising.TextLimit;
            model.BeginDate = Now.AddDays(1);
            model.EndDate = Now.AddDays((Advertising.Days+1));
            model.Position = Advertising.Position;
            model.PositionId = Advertising.PositionId;
            model.Days = Advertising.Days;
            model.bought = Advertising.DiscountPrice == null ? Advertising.SalePrice : Advertising.DiscountPrice.Value;
            var SessionKey = Guid.NewGuid().GetHashCode().ToString("x");
            Session[SessionKey] = Advertising;
            ViewBag.SessionKey = SessionKey;
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            
            var model = _db.AdOrder.Find(id);
            
            var SessionKey = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.Key = SessionKey;
            Session[SessionKey] = model.AdImage;
            ViewBag.Images = model.AdImage.ToList();
            return View(model);

        }

        [HttpPost]
        public ActionResult Edit(AdOrder model)
        {
            var result = _db.AdOrder.Find(model.ID);
            result.Text = model.Text;
            
            #region 處理圖片
            var Images = new List<AdImage>();
            if (Request.Files.Count > 0)
            {
                var Files = Request.Files;
                var UserFolder = System.Configuration.ConfigurationManager.AppSettings["UserFolder"];

                if (!Directory.Exists(Server.MapPath(UserFolder)))
                {
                    Directory.CreateDirectory(Server.MapPath(UserFolder));
                }
               
                for (var i = 0; i < Files.Count; i++)
                {
                    if (i >= model.ImageLimit)
                    {
                        break;
                    }
                    var file = Request.Files[i];
                    var Name = Guid.NewGuid().ToString();
                    var path = string.Format("~{0}{1}.jpeg", UserFolder, Name);
                    file.SaveAs(Server.MapPath(path));
                    var img = new AdImage();
                    img.Path = path;
                    img.Name = Name;
                    img.Enabled = true;
                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        img.Image = fileData;
                        Images.Add(img);
                    }
                }
            }
                #endregion

            result.AdImage = Images;

            if (User.IsInRole("Admin") || User.IsInRole("AdManager"))
            {
                result.Days = model.Days;
                result.BeginDate = model.BeginDate;
                result.EndDate = model.EndDate;                             
            }

            _db.SaveChanges();

            return RedirectToAction("List");
        }

       

      
    }
}