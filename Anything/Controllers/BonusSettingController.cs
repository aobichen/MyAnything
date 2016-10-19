using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    [Authorize(Roles="Admin")]
    public class BonusSettingController : BaseController
    {
        // GET: BonusSetting
        public ActionResult Index()
        {
            var model = _db.BonusSystem.FirstOrDefault();
            if (model != null)
            {
                model.ParentBonus = model.ParentBonus * 100;
                model.UserBonus = model.UserBonus * 100;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(BonusSystem model)
        {
            var isExist = _db.BonusSystem.Any(o => o.ID == 1);
            if (ModelState.IsValid) {
                if (!isExist)
                {
                    //model.Created = DateTime.Now;
                    model.ParentBonus = model.ParentBonus * 0.01;
                    model.UserBonus = model.UserBonus * 0.01;
                    _db.BonusSystem.Add(model);
                    _db.SaveChanges();
                }
                else
                {
                    if (!User.Identity.IsAuthenticated || string.IsNullOrEmpty(CurrentUser.UserName))
                    {
                        ModelState.AddModelError("","修改人員無法辨識");
                        return View();
                    }
                    var m = _db.BonusSystem.FirstOrDefault();
                    m.Modifiter = CurrentUser.UserName;
                    m.Modified = DateTime.Now;
                    m.UserBonus = model.UserBonus * 0.01;
                    m.ParentBonus = model.ParentBonus * 0.01;
                    _db.SaveChanges();
                }
            }
            else
            {
                ModelState.AddModelError("","錯誤的設定");
            }
            return View();
        }
    }
}