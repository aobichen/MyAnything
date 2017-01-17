using Anything.Controllers;
using Anything.Helpers;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace Anything.Areas.Hotel.Controllers
{
    public class HotelController : BaseController
    {
        public HotelController()
        {
            var City = new Caches().TWCity;

            SelectList selectList = new SelectList(City, "ID", "Name", 0);
            ViewBag.City = selectList;

            var Areas = new Caches().TWArea;

            //SelectList Areas = new SelectList(City, "ID", "Name", 0);
            var AreaList = Areas.Select(o => new DropDownListItem
            {
                DataAttr = o.CID.ToString(),
                Selected = false,
                Text = o.Name,
                Value = o.ID.ToString()
            }).ToList();
            ViewBag.Area = AreaList;

            var Location = new Caches().TWLocation;
            SelectList Locations = new SelectList(Location, "Location", "Location", 0);
            ViewBag.Location = Locations;
        }
        //
        // GET: /Hotel/
        [Authorize(Roles = "Hotel,Admin,System")]
        public ActionResult Index(int Page = 1)
        {

            var hotel = _db.Hotel.Where(o => o.UserId == CurrentUser.Id)
                .Select(o => new HotelListModel
                {
                    ID = o.ID,
                    Enabled = o.Enabled,
                    Location = o.Location,
                    Name = o.Name,
                    Qty = o.Room.Count,

                }).ToList();

            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 10;

            var PageModel = hotel.ToPagedList(currentPage, PageSize);
            ViewData.Model = PageModel;

            return View();
        }

        [Authorize(Roles = "Hotel,Admin")]
        public ActionResult Create()
        {
            var model = new HotelCreateViewModel();
            model.Information = new HtmlContent("/Views/Hotel/InformationTemp.html").Text;

            ViewBag.Facility = new FacilityModel().SelectListItems;
            ViewBag.Scenics = new ScenicModel().SelectListItems;
            ViewBag.ImgKey = Guid.NewGuid().GetHashCode().ToString("x");
            model.Enabled = true;
            return View(model);
        }



        [Authorize(Roles = "Hotel,Admin")]
        public ActionResult Edit(int? id = null)
        {

            //ViewBag.Facility = new FacilityModel().SelectListItems;
            //ViewBag.Scenics = new ScenicModel().SelectListItems;
            var result = new HotelCreateViewModel();

            var model = _db.Hotel.Where(o => o.ID == id).FirstOrDefault();

            #region ## 設施/景點
            if (model != null && !string.IsNullOrEmpty(model.Facility))
            {
                var chkoptions = model.Facility.Split(',').ToList();
                var FacilityModel = new FacilityModel();
                FacilityModel.SelectedItems = chkoptions;
                ViewBag.Facility = new FacilityModel().List(chkoptions);
            }
            else
            {
                ViewBag.Facility = new FacilityModel().SelectListItems;
            }

            if (model != null && !string.IsNullOrEmpty(model.Scenics))
            {
                var chkScenics = model.Scenics.Split(',').ToList();
                ViewBag.Scenics = new ScenicModel().List(chkScenics);
            }
            else
            {
                ViewBag.Scenics = new ScenicModel().SelectListItems;
            }
            #endregion

            if (id == null || model == null)
            {
                result.Information = new HtmlContent("/Views/Hotel/InformationTemp.html").Text;

                ViewBag.ImgKey = Guid.NewGuid().GetHashCode().ToString("x");
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



            ViewBag.CurrentCity = model.City;
            ViewBag.CurrentArea = model.Area;
            ViewBag.CurrentLocation = model.Location;
            var SessionKey = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.ImgKey = SessionKey;
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
                    return RedirectToAction("Edit", model.ID);
                }

            }

            //var CheckboxForScenics = new ScenicsCheckbox().ConverToCheckbox(model.Scenics.Split(','));
            //ViewBag.Scenics = CheckboxForScenics;
            //var CheckboxForServiceoption = new ServiceOptionCheckbox().ConverToCheckbox(model.ServiceOptions.Split(','));
            //ViewBag.ServiceOptions = CheckboxForServiceoption;
            return View(model.ID);
        }



        public ActionResult Image(int id)
        {
            var Image = _db.HotelImage.Find(id);
            return File(Image.Image, "image/jpg");
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