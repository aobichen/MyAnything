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
        public ActionResult Index(int Page =1)
        {
            var model = new AccountingViewModel().Query();
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
            
            //var SelectList = new List<SelectListItem>();
            //var SelectedItems = SelectedItems;
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
            return View();
          
        }
    }
}