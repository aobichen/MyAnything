using Anything.Areas.System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Anything.Controllers;
using Anything.Helpers;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace Anything.Areas.Hotel.Controllers
{
    [Authorize]
    public class AccountingController : BaseController
    {
        // GET: System/Accounting
        public ActionResult Index(AccountingSearchModel Search = null, int Page = 1)
        {
            var Accounting = new AccountingViewModel();
            Accounting.UserId = CurrentUser.Id;
            var model = Accounting.Query(Search);
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;

            var result = model.Accounting.ToPagedList(currentPage, PageSize);

            var PayStatus = Enum.GetNames(typeof(OrderType)).ToList();
            var PaySelectListItems = new List<SelectListItem>();

            for (var i = 0; i < PayStatus.Count; i++)
            {
                PaySelectListItems.Add(new SelectListItem
                {
                    Text = new BaseDLL().ParsePayStatusType(PayStatus[i]),
                    Value = PayStatus[i],
                    Selected = Search == null ? false : (PayStatus[i] == Search.Status)
                });
            }

            ViewBag.PayStatus = PaySelectListItems;

            #region ## Hotels DropDownList
            var Items = _db.Hotel.ToList();
            var SelectListItems = new List<SelectListItem>();

            foreach (var item in Items)
            {
                SelectListItems.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ID.ToString(),
                    Selected = (Search == null || Search.HotelId <= 0) ? false : (item.ID == Search.HotelId)
                });
            }

            ViewBag.Hotels = SelectListItems;
            #endregion

            ViewData.Model = result;
            var Today = DateTime.Now;
            ViewBag.BeginDate = (Search == null || string.IsNullOrEmpty(Search.BeginDate) || DateTime.Parse(Search.BeginDate) <= DateTime.MinValue) ? Today.ToString("yyyy-MM-01") : Search.BeginDate;
            ViewBag.EndDate = (Search == null || string.IsNullOrEmpty(Search.EndDate) || DateTime.Parse(Search.EndDate) <= DateTime.MinValue) ? Today.ToString("yyyy-MM") + "-" + DateTime.DaysInMonth(Today.Year, Today.Month).ToString() : Search.EndDate;
            ViewBag.KeyWord = Search.Keyword;
            ViewBag.HotelId = Search.HotelId;
            ViewBag.Status = Search.Status;

            ViewBag.AmoutTotal = model.AmoutTotal;
            ViewBag.IncomeTotal = model.IncomeTotal;

            return View();

        }

        public ActionResult ExportAccountingExcel()
        {
            if (Session["AccountingExcel"] != null)
            {
                //var list = _basedb.ReportRooms.ToList();
                //DataTable dt = ConvertListToDataTable(list);
                var AccountingExcelModel = (List<AccountingExcelModel>)Session["AccountingExcel"];

                var AmoutTotal = AccountingExcelModel.Sum(o => o.實付金額).ToString("#,##0");
                var IncomeTotal = AccountingExcelModel.Sum(o => o.業者實收金額).ToString("#,##0");

                DataTable dt = new AccountingExcelModel().ConvertListToDataTable(AccountingExcelModel);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //var worksheet = wb.Worksheet();
                    //wb.Worksheet("報表");
                    wb.Worksheets.Add(dt, "報表");
                    wb.Worksheets.First().Cell(dt.Rows.Count + 2, 1).SetValue(string.Format("訂單總額: {0}", AmoutTotal));
                    wb.Worksheets.First().Cell(dt.Rows.Count + 2, 2).SetValue(string.Format("實收總額 : {0}", IncomeTotal));
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    var FileName = Guid.NewGuid().GetHashCode().ToString("x");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= " + FileName + ".xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }

            }
            return RedirectToAction("Export", "Report");
        }
    }
}