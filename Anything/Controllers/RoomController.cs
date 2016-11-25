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
   
    public class RoomController : BaseController
    {
        // GET: Room
        public ActionResult Index(int id)
        {
            if (!_db.Hotel.Any(o => o.ID == id))
            {
                return RedirectToAction("Index", "Hotel");
            }

            var Hotel = _db.Hotel.Find(id);
            if (Hotel.UserId != CurrentUser.Id)
            {
                return RedirectToAction("Index", "Hotel");
            }

            var rooms = _db.Room.Where(o => o.HotelId == id).ToList();
            ViewBag.HotelId = id;

            return View(rooms);
        }

        public ActionResult Create(int id)
        {
            var Hotel = _db.Hotel.Find(id);
            if (Hotel == null || Hotel.UserId != CurrentUser.Id)
            {
                return RedirectToAction("Login","Account");
            }
            
            var model = new RoomCreateViewModel();
            //model.Bonus = 300;
            ViewBag.SessionKey = Guid.NewGuid().GetHashCode().ToString("x");
            model.HotelId = id;
            model.Enabled = true;
            model.Quantity = 1;
            model.BedAmount = 1;
            model.Notice = new HtmlContent("/Views/Room/InformationTemp.html").Text;
           
            ViewBag.BedType =new CodeFiles().GetBedsSelectList();


            ViewBag.RoomType = new CodeFiles().GetRoomsSelectList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RoomCreateViewModel model)
        {

            //if (model.ID > 0)
            //{
            //    model.Edit();
            //    return RedirectToAction("Edit", new {id=model.ID });
            //}

            var RoomImage = new List<RoomImage>();
            var PersonBed = new List<int>();
            //PersonBed.Add(model.m);
            //PersonBed.Add(int.Parse(model.BedType));
            //PersonBed.Add(model.Beds);
            //[房型,床型,數量]
            model.RoomBed = string.Join(",", PersonBed);
           
             RoomImage = (List<RoomImage>)Session[model.SessionKey];
             Session[model.SessionKey] = RoomImage;
                
             ViewBag.RoomImage = RoomImage;
               
            

            if (RoomImage == null || RoomImage.Count <= 0)
            {
                ModelState.AddModelError("","必須上傳圖片");
                return View();
            }

            //if (string.IsNullOrEmpty(model.Notice))
            //{
            //    ModelState.AddModelError("", "必須提供房型資訊");
            //    return View();
            //}

            

            model.UserId = CurrentUser.Id;

            if (ModelState.IsValid){
                model.Create();
                return RedirectToAction("Index", new {id=model.HotelId});
            }
            return View(model);
        }


        public ActionResult SetPrice(int id)
        {
            var Room = _db.Room.Find(id);
            if (Room == null || Room.Hotel.UserId != CurrentUser.Id)
            {
                return RedirectToAction("Login","Account");
            }

            ViewBag.Room = Room;

            ViewBag.RoomId = Room.ID;
            ViewBag.Name = string.Format("{0}/{1}", Room.Hotel.Name,Room.Name); 
            return View();
        }

        public ActionResult Edit(int id)
        {
            
            
            var room = _db.Room.Find(id);
            if (room == null || room.Hotel.UserId != CurrentUser.Id)
            {
                return RedirectToAction("","Home");
            }
            var Images = room.RoomImage.ToList();

            var model = (from r in _db.Room
                         where (r.ID == id)
                         select new RoomCreateViewModel
                         {
                             ID = r.ID,
                             HotelId = r.HotelId,
                             BedType = r.BedType,
                             FixedPrice = r.FixedPrice,
                             HolidayPrice = r.HolidayPrice,
                             DayPrice = r.DayPrice,
                             Enabled = r.Enabled,
                             Quantity = r.Quantity,
                             RoomType = r.RoomType,
                             Notice = r.Notice,
                             Name = r.Name,
                             MaxPerson = r.MaxPerson
                         }).FirstOrDefault();
            
            var key = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.SessionKey = key;
            Session[key] = Images;
            ViewBag.RoomImages = Images;
            model.SessionKey = key;
            ViewBag.BedType = new CodeFiles().GetBedsSelectList(model.BedType);
         
            ViewBag.RoomType = new CodeFiles().GetRoomsSelectList(model.RoomType);
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(RoomCreateViewModel model)
        {
            var room = _db.Room.Find(model.ID);
            if (room == null || room.Hotel.UserId != CurrentUser.Id)
            {
                return RedirectToAction("", "Home");
            }
            try
            {
                model.Edit();
                
            }
            catch(Exception ex){
                ViewBag.BedType = new CodeFiles().GetBedsSelectList(model.BedType);
                ViewBag.RoomType = new CodeFiles().GetRoomsSelectList(model.RoomType);
                ModelState.AddModelError("",ex.Message.ToString());
                return View();
            }

            return RedirectToAction("Edit", new { id = model.ID });
        }

        
        public ActionResult Image(int id)
        {
            var Image = _db.RoomImage.Find(id);
            return File(Image.Image, "image/jpg");
        }
    }
}