using Anything.Areas.System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Anything.Controllers;
using Anything.Helpers;
namespace Anything.Areas.System.Controllers
{
    public class AccountingController : BaseController
    {
        // GET: System/Accounting
        public ActionResult Index(AccountingSearchModel Search = null,int Page = 1)
        {
            var model = new AccountingViewModel().Query(Search);
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 10;

            var result = model.ToPagedList(currentPage, PageSize);

            var Items = _db.Hotel.ToList();

            var PayStatus = Enum.GetNames(typeof(OrderType)).ToList();
            var PaySelectListItems = new List<SelectListItem>();

            for (var i = 0; i < PayStatus.Count; i++)
            {
                PaySelectListItems.Add(new SelectListItem
                {
                    Text = new BaseDLL().ParsePayStatusType(PayStatus[i]),
                    Value = PayStatus[i],

                });
            }

            ViewBag.PayStatus = PaySelectListItems;
           
            var SelectListItems = new List<SelectListItem>();
            
            foreach (var item in Items)
            {
                SelectListItems.Add(new SelectListItem
                { 
                    Text = item.Name,
                    Value = item.ID.ToString(),
                                    
                });
            }

            ViewBag.Hotels = SelectListItems; 
            ViewData.Model = result;
            var Today = DateTime.Now;
            ViewBag.BeginDate = (Search == null || Search.BeginDate <= DateTime.MinValue) ? DateTime.Parse(Today.ToString("yyyy-MM-01")) : Search.BeginDate;
            ViewBag.EndDate = (Search == null || Search.EndDate <= DateTime.MinValue) ? DateTime.Parse(Today.ToString("yyyy-MM") + "-" + DateTime.DaysInMonth(Today.Year, Today.Month).ToString()).AddSeconds(-1) : Search.EndDate;
            return View();
          
        }
    }
}