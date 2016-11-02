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
            var model = new RoomCreateViewModel();
            //model.Bonus = 300;
            ViewBag.SessionKey = Guid.NewGuid().GetHashCode().ToString("x");
            model.HotelId = id;
            model.Enabled = true;
            model.Amount = 1;
            model.Beds = 1;
            model.Information = new HtmlContent("/Views/Room/InformationTemp.html").Text;
           
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
            PersonBed.Add(model.Person);
            PersonBed.Add(int.Parse(model.BedType));
            PersonBed.Add(model.Beds);
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

            if (string.IsNullOrEmpty(model.Information))
            {
                ModelState.AddModelError("", "必須提供房型資訊");
                return View();
            }

            

            model.UserId = CurrentUser.Id;

            if (ModelState.IsValid){
                model.Create();
                return RedirectToAction("Index", new {id=model.HotelId});
            }
            return View(model);
        }


        public ActionResult SetPrice(int id)
        {
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
                             Beds = r.Beds,
                             SellPrice = r.SellPrice,
                             Enabled = r.Enabled,
                             Amount = r.Amount,
                             BedType = r.BedType,
                             Bonus = r.Bonus,
                             DiscountPrice = r.DiscountPrice,
                             Information = r.Information,
                             Name = r.Name,
                             Person = r.Person
                         }).FirstOrDefault();
            
            var key = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.SessionKey = key;
            Session[key] = Images;
            ViewBag.RoomImages = Images;
            model.SessionKey = key;
            ViewBag.BedType = new CodeFiles().GetBedsSelectList(model.BedType);
         
            ViewBag.RoomType = new CodeFiles().GetRoomsSelectList(model.Person.ToString());
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
                ViewBag.RoomType = new CodeFiles().GetRoomsSelectList(model.Person.ToString());
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