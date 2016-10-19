using Anything.Helper;
using Anything.Models;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    [Authorize]
    public class RoomController : BaseController
    {
        // GET: Room
        public ActionResult Index(int id)
        {
            if (!_db.Hotel.Any(o => o.ID == id))
            {
                return RedirectToAction("Index", "Hotel");
            }
            var rooms = _db.Room.Where(o => o.HotelId == id).ToList();
            ViewBag.HotelId = id;

            return View(rooms);
        }

        public ActionResult Create(int id)
        {
            var model = new RoomCreateViewModel();
            model.Bonus = 300;
            ViewBag.SessionKey = Guid.NewGuid().GetHashCode().ToString("x");
            model.HotelId = id;
            model.Enabled = true;
            model.Amount = 1;
            model.Information = new HtmlContent("/Views/Room/InformationTemp.html").Text;
           
            ViewBag.BedType =new CodeFiles().GetBedsSelectList();


            ViewBag.RoomType = new CodeFiles().GetRoomsSelectList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RoomCreateViewModel model)
        {
            var RoomImage = new List<RoomImage>();
            var PersonBed = new List<string>();
            PersonBed.Add(model.Person);
            PersonBed.Add(model.BedType);
            PersonBed.Add(model.Beds.ToString());
            //[房型,床型,數量]
            model.RoomBed = string.Join(",", PersonBed);
            if (Request["imagekey"] != null)
            {
                RoomImage = (List<RoomImage>)Session[Request["imagekey"]];
                Session[Request["imagekey"]] = RoomImage;
                for (var i = 0; i < RoomImage.Count; i++)
                {
                    RoomImage[i].Sort = i + 1;
                    //RoomImage[i].Enabled = true;
                }

                ViewBag.RoomImage = RoomImage;
               
            }

            if (RoomImage == null || RoomImage.Count <= 0)
            {
                ModelState.AddModelError("","必須上傳圖片");
                return View();
            }

            if (string.IsNullOrEmpty(model.Information))
            {
                ModelState.AddModelError("", "必須提供房型資訊");
                return View();
            }
            var Now = DateTime.Now;
            var bussinessbouns = 0;
            var platformbouns = 0;
            if (model.DiscountPrice == null)
            {
                bussinessbouns = int.Parse((double.Parse(model.SellPrice.ToString()) * _Bussiness).ToString());
                platformbouns = int.Parse((double.Parse(model.SellPrice.ToString()) * _Platform).ToString());
            }
            else
            {
                bussinessbouns = int.Parse((double.Parse(model.DiscountPrice.ToString()) * _Bussiness).ToString());
                platformbouns = int.Parse((double.Parse(model.DiscountPrice.ToString()) * _Platform).ToString());
            }

            if (ModelState.IsValid)
            {
                _db.Room.Add(new Room
                {
                    Amount = model.Amount,
                    Bonus = model.Bonus,
                  BussinessBonus = bussinessbouns,
                  PlatformBonus = platformbouns,
                    HotelId = model.HotelId,
                    Created = Now,
                    DiscountPrice = model.DiscountPrice,
                    Creator = CurrentUser.Id,
                    Enabled = model.Enabled,
                    Information = model.Information,
                    Modified = Now,
                    Name = model.Name,
                    SellPrice = model.SellPrice,
                    RoomImage = RoomImage,
                    Beds = model.Beds,
                    BedType = model.BedType,
                    Person =int.Parse(model.Person),
                    RoomBed = model.RoomBed
                });
                _db.SaveChanges();
                return RedirectToAction("Index", new {id=model.HotelId});
            }
            return View(model);
        }


        public ActionResult RoomSet()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var model = _db.Room.Find(id);
            var key = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.SessionKey = key;
            Session[key] = model.RoomImage.ToList();
            ViewBag.RoomImg = model.RoomImage.ToList();
           
            ViewBag.BedType = new CodeFiles().GetBedsSelectList(model.BedType);
         
            ViewBag.RoomType = new CodeFiles().GetRoomsSelectList(model.Person.ToString());
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Room model)
        {
            var result = _db.Room.Find(model.ID);
            var PersonBed = new List<string>();
            PersonBed.Add(model.Person.ToString());
            PersonBed.Add(model.BedType);
            PersonBed.Add(model.Beds.ToString());
            //[房型,床型,數量]
            model.RoomBed = string.Join(",", PersonBed);
            if (result == null)
            {
                return Redirect("Index");
            }


            var InsertRoomlImage = new List<RoomImage>();

            if (Request["imagekey"] != null)
            {
                var RoomImages = (List<RoomImage>)Session[Request["imagekey"]];
                Session[Request["imagekey"]] = RoomImages;
                var ImgFromDb = _db.RoomImage.Where(o => o.RoomId == model.ID).ToList();
                var NameOfImages = _db.RoomImage.Where(o => o.RoomId == model.ID).Select(x => x.Name).ToList();
                var MaxSort = 0;
               
                for (var i = 0; i < ImgFromDb.Count; i++)
                {
                    var sort = i + 1;
                    ImgFromDb[i].Sort = sort;
                    if (sort >= ImgFromDb.Count)
                    {
                        MaxSort = i + 1;
                    }
                }

                InsertRoomlImage = RoomImages.Where(o => !NameOfImages.Contains(o.Name)).ToList();
                if (InsertRoomlImage.Count > 0)
                {
                    for (var i = 0; i < InsertRoomlImage.Count; i++)
                    {
                        InsertRoomlImage[i].Sort = MaxSort + (i+1);
                        
                    }
                }

                //InsertRoomlImage = InsertRoomlImage.Select(o => { o.Room = null; return o; }).ToList();

            }

            var bussinessbouns = 0;
            var platformbouns = 0;
            if (model.DiscountPrice == null)
            {
                bussinessbouns = int.Parse((double.Parse(model.SellPrice.ToString()) * _Bussiness).ToString());
                platformbouns = int.Parse((double.Parse(model.SellPrice.ToString()) * _Platform).ToString());
            }
            else
            {
                bussinessbouns = int.Parse((double.Parse(model.DiscountPrice.ToString()) * _Bussiness).ToString());
                platformbouns = int.Parse((double.Parse(model.DiscountPrice.ToString()) * _Platform).ToString());
            }

            if (ModelState.IsValid)
            {
                result.HotelId = model.HotelId;
                result.Information = model.Information;
                result.Modified = DateTime.Now;
                result.Name = model.Name;
                result.BussinessBonus = bussinessbouns;
                result.PlatformBonus = platformbouns;
               // result.RoomImage = InsertRoomlImage;
                result.SellPrice = model.SellPrice;
                result.DiscountPrice = model.DiscountPrice;
                result.Amount = model.Amount;
                result.Bonus = model.Bonus;
                result.Enabled = model.Enabled;
                result.Person = model.Person;
                result.RoomBed = model.RoomBed;
                result.Beds = model.Beds;
                result.BedType = model.BedType;
                _db.SaveChanges();

                if (InsertRoomlImage != null && InsertRoomlImage.Count > 0)
                {
                    InsertRoomlImage = InsertRoomlImage.Select(c => { c.RoomId = model.ID; c.Enabled = true; return c; }).ToList();

                    new ImageHandle().CreateRoomImage(InsertRoomlImage);
                }
                return RedirectToAction("Index",new{id=model.HotelId});
            }
            return View();
        }

        
        public ActionResult Image(int id)
        {
            var Image = _db.RoomImage.Find(id);
            return File(Image.Image, "image/jpg");
        }
    }
}