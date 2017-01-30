using Anything.Areas.My.Models;
using Anything.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Anything.ViewModels;
namespace Anything.Areas.My.Controllers
{
    [Authorize]
    public class BonusController : BaseController
    {
        // GET: My/Bonus
        public ActionResult Index(MyBonusSearchModel model = null,int Page = 1)
        {
            var Status = string.Empty;
            switch(model.Status){
                case 3:
                    Status = BonusStatusEnum.CanUse.ToString();
                    break;
                case 2:
                    Status = BonusStatusEnum.CurrentBonus.ToString();
                    break;
                case 4:
                    Status = BonusStatusEnum.NoOverAmount.ToString();
                    break;
                case 1:
                    Status = "ALL";
                    break;
                
            }
            var today = DateTime.Now;
            var lastDate = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            ViewBag.begindate = today.ToString("yyyy-MM") + "-01";
            ViewBag.enddate = today.ToString("yyyy-MM-") + lastDate.ToString("dd");
            var result = new List<MyBonusModel>();
            if (model.Status == 0 && model.BeginDate == DateTime.MinValue && model.EndDate == DateTime.MinValue)
            {
                result = _db.MyBonus.Where(o => o.UserID == CurrentUser.Id &&
                       (o.UseMonth.Year == today.Year && o.UseMonth.Month == today.Month)).Select(o => new MyBonusModel
                       {
                           Amt = o.Bonus,
                           Date = o.Created,
                           Status = o.BonusStatus,
                           Type = o.BonusType,
                           ID = o.ID,
                           UserId = o.UserID
                       }).ToList();
            }
            else
            {
                result = _db.MyBonus.Where(o => o.UserID == CurrentUser.Id &&
                       (o.UseMonth.Year == today.Year && o.UseMonth.Month == today.Month)).Select(o => new MyBonusModel
                       {
                           Amt = o.Bonus,
                           Date = o.Created,
                           Status = o.BonusStatus,
                           Type = o.BonusType,
                           ID = o.ID,
                           UserId = o.UserID
                       }).ToList();
            }

            ViewBag.Sum = result.Sum(o=>o.Amt).ToString("#.##");

            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 25;

            var PageModel = result.ToPagedList(currentPage, PageSize);

            ViewData.Model = PageModel;
            return View();
        }
    }
}