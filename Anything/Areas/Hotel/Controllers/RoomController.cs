using Anything.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Anything.ViewModels;
using Anything.Helpers;
using Anything.Models;
namespace Anything.Areas.Hotel.Controllers
{
    public class RoomController : BaseController
    {
        [Authorize]
        public ActionResult Index(int id, int Page = 1)
        {

            var Hotel = _db.Hotel.Where(o => o.ID == id && o.UserId == CurrentUser.Id).FirstOrDefault();
            if (Hotel == null)
            {
                return RedirectToAction("Index", "Hotel");
            }


            var rooms = _db.Room.Where(o => o.HotelId == id).Select(o => new RoomModel
            {
                Name = o.Name,
                Quantity = o.Quantity,
                Enabled = o.Enabled,
                FixedPrice = o.FixedPrice,
                DayPrice = o.DayPrice,
                HolidayPrice = o.HolidayPrice,
                ID = o.ID
            }).ToList();


            ViewBag.HotelId = id;
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 10;

            var PageModel = rooms.ToPagedList(currentPage, PageSize);
            ViewData.Model = PageModel;
            return View();
        }

        public ActionResult Create(int id)
        {
            var Hotel = _db.Hotel.Find(id);
            if (Hotel == null || Hotel.UserId != CurrentUser.Id)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new RoomCreateViewModel();
            //model.Bonus = 300;
            var ImgKey = Guid.NewGuid().GetHashCode().ToString("x");
            model.ImgKey = ImgKey;
            model.HotelId = id;
            model.Enabled = true;
            model.Quantity = 1;
            model.BedAmount = 1;
            model.Notice = new HtmlContent("/Views/Room/InformationTemp.html").Text;

            ViewBag.BedType = new CodeFiles().GetBedsSelectList();


            ViewBag.RoomType = new CodeFiles().GetRoomsSelectList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RoomCreateViewModel model)
        {

            var RoomImage = new List<RoomImage>();
            var PersonBed = new List<int>();

            RoomImage = (List<RoomImage>)Session[model.ImgKey];
            Session[model.ImgKey] = RoomImage;

            ViewBag.RoomImage = RoomImage;


            model.UserId = CurrentUser.Id;

            if (ModelState.IsValid)
            {
                model.Create();
                return RedirectToAction("Index", new { id = model.HotelId });
            }

            ViewBag.BedType = new CodeFiles().GetBedsSelectList();
            ViewBag.RoomType = new CodeFiles().GetRoomsSelectList();
            return View(model);
        }


        public ActionResult SetPrice(int id)
        {
            var Room = _db.Room.Find(id);
            if (Room == null || Room.Hotel.UserId != CurrentUser.Id)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Room = Room;

            ViewBag.RoomId = Room.ID;
            ViewBag.Name = string.Format("{0}/{1}", Room.Hotel.Name, Room.Name);
            return View();
        }

        public ActionResult Edit(int id)
        {


            var room = _db.Room.Find(id);
            if (room == null || room.Hotel.UserId != CurrentUser.Id)
            {
                return RedirectToAction("", "Home");
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
                             MaxPerson = r.MaxPerson,
                             BedAmount = r.BedAmount,
                             Feature = r.Feature
                         }).FirstOrDefault();

            var key = Guid.NewGuid().GetHashCode().ToString("x");
            model.ImgKey = key;
            //ViewBag.SessionKey = key;
            Session[key] = Images;
            //ViewBag.RoomImages = Images;
            ViewBag.ImgKey = key;
            ViewBag.BedType = new CodeFiles().GetBedsSelectList(model.BedType);

            ViewBag.RoomType = new CodeFiles().GetRoomsSelectList(model.RoomType);
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(RoomCreateViewModel model)
        {
            var room = _db.Room.Find(model.ID);
            model.Creator = CurrentUser.Id;
            if (room == null || room.Hotel.UserId != CurrentUser.Id)
            {
                return RedirectToAction("", "Home");
            }
            try
            {
                model.Edit();
            }
            catch (Exception ex)
            {
                ViewBag.BedType = new CodeFiles().GetBedsSelectList(model.BedType);
                ViewBag.RoomType = new CodeFiles().GetRoomsSelectList(model.RoomType);
                ModelState.AddModelError("", ex.Message.ToString());
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