using Anything.Helper;
using Anything.Models;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
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

        //[Authorize(Roles = "Hotel,Admin")]
        //[HttpPost]
        //public ActionResult Create(HotelCreateViewModel model)
        //{
        //    if (model.City <= 0)
        //    {
        //        ModelState.AddModelError("City","城市為必填");
        //    }
        //    if (model.Area <= 0)
        //    {
        //        ModelState.AddModelError("Area", "鄉鎮區為必填");
        //    }
        //    if (Request["ServiceOptions"] != null)
        //    {
        //        model.ServiceOptions = Request["ServiceOptions"];
        //    }

        //    if (Request["Scenics"] != null)
        //    {
        //        model.Scenics = Request["Scenics"];
        //    }

        //    var HotelImage = new List<HotelImage>();
        //    if (Request["imagekey"] != null)
        //    {
        //        HotelImage = (List<HotelImage>)Session[model.SessionKey];
        //        Session[model.SessionKey] = HotelImage;
        //        var UserFolder = System.Configuration.ConfigurationManager.AppSettings["UserFolder"];
        //        for (var i=0;i<HotelImage.Count;i++)
        //        {
        //            var fileName = Guid.NewGuid().ToString();
        //            var webPath = Path.Combine(UserFolder, fileName + ".jpg");
        //            var path = Path.Combine(Server.MapPath(UserFolder), fileName + ".jpg");
        //            MemoryStream ms = new MemoryStream(HotelImage[i].Image);
        //            Image returnImage =System.Drawing.Image.FromStream(ms);
        //            HotelImage[i].Sort = i+1;
        //            HotelImage[i].Enabled = true;
        //        }

        //        ViewBag.HotelImg = HotelImage;
        //    }
           
        //    if (ModelState.IsValid)
        //    {
        //        var Now = DateTime.Now;
        //        var db_model = new Hotel() {
        //            Address = model.Address,
        //            City = model.City,
        //            Area = model.Area, 
        //            Created = Now, 
        //            Enabled = true,
        //            Feature = model.Feature,
        //            Information = model.Information,
        //            Introduce = model.Introduce,
        //            Location = model.Location, 
        //            Name = model.Name, 
        //            ServiceOptions = model.ServiceOptions,
        //            Scenics = model.Scenics,
        //            UserId = CurrentUser.Id,
        //            WebSite = model.WebSite,
        //            Modified = Now,
        //            HotelImage = HotelImage,
        //            Tel=  model.Tel,
        //            SaleOff = true
        //        };
        //        _db.Hotel.Add(db_model);
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    var City = new Caches().TWCity;
        //    ViewBag.Area = new Caches().TWArea;
        //    SelectList selectList = new SelectList(City, "ID", "City", 0);
        //    ViewBag.City = selectList;
        //    ViewBag.Area = new Caches().TWArea;
        //    ViewBag.ServiceOptions = _db.ServiceOption.Where(o => o.Enabled == true).ToList();
        //    ViewBag.Scenics = _db.Scenic.Where(o => o.Enabled == true).ToList();

        //    return View();
        //}

        [Authorize(Roles = "Hotel,Admin")]
        public ActionResult Edit(int? id=null)
        {


            var result = new HotelCreateViewModel();

            var model = _db.Hotel.Where(o => o.ID == id).FirstOrDefault();
            if (id == null || model == null)
            {
                result.Information = new HtmlContent("/Views/Hotel/InformationTemp.html").Text;
                ViewBag.ServiceOptions = new ServiceOptionCheckbox().ConverToCheckbox(null);
                ViewBag.Scenics = new ScenicsCheckbox().ConverToCheckbox(null);
                ViewBag.SessionKey = Guid.NewGuid().GetHashCode().ToString("x");
                result.Enabled = true;
                result.SaleOff = true;
                return View(result);
            }

            result.ID = model.ID;
            result.Information = model.Information;
            result.Introduce = model.Introduce;
            result.Location = model.Location;
            result.Name = model.Name;
            result.Tel = model.Tel;
            result.WebSite = model.WebSite;
            result.Enabled = model.Enabled;
            result.Feature = model.Feature;
            result.Area = model.Area;
            result.City = model.City;
            result.Address = model.Address;
            result.SaleOff = model.SaleOff;
            result.Enabled = true;
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

            

            return View(result);
        }

        [Authorize(Roles = "Hotel,Admin")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(HotelCreateViewModel model)
        {
            model.UserId = CurrentUser.Id;
            


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

            if (ModelState.IsValid)
            {
                if (model.ID == 0)
                {
                    model.Create();
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Edit();
                    return RedirectToAction("Edit",model.ID);
                }
               
            }

            var CheckboxForScenics = new ScenicsCheckbox().ConverToCheckbox(model.Scenics.Split(','));
            ViewBag.Scenics = CheckboxForScenics;
            var CheckboxForServiceoption = new ServiceOptionCheckbox().ConverToCheckbox(model.ServiceOptions.Split(','));
            ViewBag.ServiceOptions = CheckboxForServiceoption;
            return View(model.ID);
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