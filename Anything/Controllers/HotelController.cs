using Anything.Helper;
using Anything.Models;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    
    public class HotelController : BaseController
    {
        //
        // GET: /Hotel/
        [Authorize(Roles = "Hotel,Admin")]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated || CurrentUser == null || CurrentUser.Id == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var hotel = _db.Hotel.Where(o => o.UserId == CurrentUser.Id).ToList();

            return View(hotel);
        }

        [Authorize(Roles = "Hotel,Admin")]
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated || CurrentUser == null || CurrentUser.Id == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var City = new Caches().TWCity;
            ViewBag.Area = new Caches().TWArea;
            SelectList selectList = new SelectList(City, "ID", "City", 0);
            ViewBag.City = selectList;
            ViewBag.Area = new Caches().TWArea;

            
            var model = new HotelCreateViewModel();
            model.Information = new HtmlContent("/Views/Hotel/InformationTemp.html").Text;

            ViewBag.ServiceOptions = _db.ServiceOption.Where(o => o.Enabled == true).ToList();
            ViewBag.Scenics = _db.Scenic.Where(o => o.Enabled == true).ToList();
            ViewBag.SessionKey = Guid.NewGuid().GetHashCode().ToString("x");
            model.Enabled = true;
            return View(model);
        }

        [Authorize(Roles = "Hotel,Admin")]
        [HttpPost]
        public ActionResult Create(HotelCreateViewModel model)
        {
            if (model.City <= 0)
            {
                ModelState.AddModelError("City","城市為必填");
            }
            if (model.Area <= 0)
            {
                ModelState.AddModelError("Area", "鄉鎮區為必填");
            }
            if (Request["ServiceOptions"] != null)
            {
                model.ServiceOptions = Request["ServiceOptions"];
            }

            if (Request["Scenics"] != null)
            {
                model.Scenics = Request["Scenics"];
            }

            var HotelImage = new List<HotelImage>();
            if (Request["imagekey"] != null)
            {
                HotelImage = (List<HotelImage>)Session[Request["imagekey"]];
                Session[Request["imagekey"]] = HotelImage;
                for (var i=0;i<HotelImage.Count;i++)
                {
                    HotelImage[i].Sort = i+1;
                    HotelImage[i].Enabled = true;
                }

                ViewBag.HotelImg = HotelImage;
            }
           
            if (ModelState.IsValid)
            {
                var Now = DateTime.Now;
                var db_model = new Hotel() {
                    Address = model.Address,
                    City = model.City,
                    Area = model.Area, 
                    Created = Now, 
                    Enabled = model.Enabled,
                    Feature = model.Feature,
                    Information = model.Information,
                    Introduce = model.Introduce,
                    Location = model.Location, 
                    Name = model.Name, 
                    ServiceOptions = model.ServiceOptions,
                    Scenics = model.Scenics,
                    UserId = CurrentUser.Id,
                    WebSite = model.WebSite,
                    Modified = Now,
                    HotelImage = HotelImage,
                    Tel=  model.Tel,
                    SaleOff = true
                };
                _db.Hotel.Add(db_model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            var City = new Caches().TWCity;
            ViewBag.Area = new Caches().TWArea;
            SelectList selectList = new SelectList(City, "ID", "City", 0);
            ViewBag.City = selectList;
            ViewBag.Area = new Caches().TWArea;
            ViewBag.ServiceOptions = _db.ServiceOption.Where(o => o.Enabled == true).ToList();
            ViewBag.Scenics = _db.Scenic.Where(o => o.Enabled == true).ToList();

            return View();
        }

        [Authorize(Roles = "Hotel,Admin")]
        public ActionResult Edit(int id)
        {
            if (!User.Identity.IsAuthenticated || CurrentUser == null || CurrentUser.Id == 0)
            {
                return RedirectToAction("Index", "Home");
            }
           
            var model = _db.Hotel.Where(o => o.ID == id).FirstOrDefault();
            if (model == null)
            {
                return Redirect("Index");
            }

            string[] chkoptions = null;
            if (!string.IsNullOrEmpty(model.ServiceOptions))
            {
                 chkoptions = model.ServiceOptions.Split(',');             
            }

            var CheckboxForServiceoption = new ServiceOptionCheckbox().ConverToCheckbox(chkoptions);
            ViewBag.ServiceOptions = CheckboxForServiceoption;

            string[] chkScenics = null;

            if (!string.IsNullOrEmpty(model.Scenics))
            {
                 chkScenics = model.Scenics.Split(',');
               
            }

            var CheckboxForScenics = new ScenicsCheckbox().ConverToCheckbox(chkScenics);
            ViewBag.Scenics = CheckboxForScenics;

            ViewBag.CurrentCity = model.City;
            ViewBag.CurrentArea = model.Area;
            ViewBag.CurrentLocation = model.Location;
            var SessionKey = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.SessionKey = SessionKey;
            var HotelImages = model.HotelImage.ToList();
            Session[SessionKey] = HotelImages;
            ViewBag.HotelImg = HotelImages;
            return View(model);
        }

        [Authorize(Roles = "Hotel,Admin")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Hotel model)
        {
            if (!User.Identity.IsAuthenticated || CurrentUser == null || CurrentUser.Id == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            

            var result = _db.Hotel.Find(model.ID);

            if (CurrentUser.Id != model.UserId || result.ID != model.ID)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result == null)
            {
                return Redirect("Index");
            }

            if (model.City <= 0)
            {
                ModelState.AddModelError("City", "城市為必填");
            }
            if (model.Area <= 0)
            {
                ModelState.AddModelError("Area", "鄉鎮區為必填");
            }
            if (Request["ServiceOptions"] != null)
            {
                model.ServiceOptions = Request["ServiceOptions"];
            }

            if (Request["Scenics"] != null)
            {
                model.Scenics = Request["Scenics"];
            }

            var InsertHotelImage = new List<HotelImage>();

            if (Request["imagekey"] != null)
            {
                var HotelImages = (List<HotelImage>)Session[Request["imagekey"]];
                Session[Request["imagekey"]] = HotelImages;
                var ImgFromDb = _db.HotelImage.Where(o => o.HotelId == model.ID).ToList();
                var NameOfImages = _db.HotelImage.Where(o => o.HotelId == model.ID).Select(x => x.Name).ToList();
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


                InsertHotelImage = HotelImages.Where(o => !NameOfImages.Contains(o.Name)).ToList();
                if (InsertHotelImage.Count > 0)
                {
                    for (var i = 0; i < InsertHotelImage.Count;i++ )
                    {
                        InsertHotelImage[i].Sort = MaxSort + (i+1);
                    }
                }
            }

            
            
            //model.Modified = DateTime.Now;

            if (ModelState.IsValid)
            {

                result.Information = model.Information;
                result.Introduce = model.Introduce;
                result.Location = model.Location;
                result.Modified = DateTime.Now;
                result.Name = model.Name;
                result.Scenics = model.Scenics;
                result.ServiceOptions = model.ServiceOptions;
                result.WebSite = model.WebSite;
                result.Feature = model.Feature;
                result.Address = model.Address;
                result.Area = model.Area;
                result.City = model.City;
                result.Enabled = model.Enabled;
                result.Tel = model.Tel;
                //result.HotelImage = InsertHotelImage;
                
                _db.SaveChanges();

                InsertHotelImage = InsertHotelImage.Select(c => { c.HotelId = model.ID; c.Enabled = true; return c; }).ToList();

                new ImageHandle().CreateHotelImage(InsertHotelImage);
                return RedirectToAction("Index");
            }
            return View();
        }


       
        public ActionResult Image(int id)
        {
            var Image = _db.HotelImage.Find(id);
            return File(Image.Image,"image/jpg");
        }

        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    RegisterViewModel model = new RegisterViewModel();
        //    model.UserType = "User";
        //    return View(model);

        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    model.UserCode = new Anything.Helper.Base().GetUserCode(model.UserName);
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, UserType = model.UserType, UserCode = model.UserCode };
        //        var result = await UserManager.CreateAsync(user, model.Password);

        //        if (result.Succeeded)
        //        {

        //            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //            await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
        //            ViewBag.Link = callbackUrl;
        //            return View("DisplayEmail");
        //        }
        //        AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}
	}
}