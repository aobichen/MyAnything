using Anything.Helpers;
using Anything.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Anything.Areas.System.Models
{
    public class AccountingMasterViewModel
    {
        public List<AccountingViewModel> Accounting { get; set; }

        public string AmoutTotal { get; set; }

        public string IncomeTotal { get; set; }
    }
    public class AccountingViewModel
    {
        public int ID { get; set; }
        public string HotelName { get; set; }
        public string RoomName { get; set; }
        public string OrderId { get; set; }

         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        public decimal Quantity { get; set; }
         [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public decimal Paid { get; set; }
         [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public decimal Bonus { get; set; }
         [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public decimal Income { get; set; }

        public string Status { get; set; }

        public string Total { get; set; }

        public int UserId { get; set; }

        public AccountingMasterViewModel Query(AccountingSearchModel search = null)
        {
            var MasterModel = new AccountingMasterViewModel();
            var Today = DateTime.Now;
            var BeginDate = (search == null || string.IsNullOrEmpty(search.BeginDate) || DateTime.Parse(search.BeginDate) <= DateTime.MinValue) ? Today.ToString("yyyy-MM-01") : search.BeginDate;
            var EndDate = (search == null || string.IsNullOrEmpty(search.EndDate) || DateTime.Parse(search.EndDate) <= DateTime.MinValue) ? (DateTime.Parse(Today.ToString("yyyy-MM") +"-"+DateTime.DaysInMonth(Today.Year,Today.Month).ToString()).AddSeconds(-1)).ToString() : search.EndDate;

            var BeginDateOfSearch = DateTime.Parse(BeginDate);
            var EndDateOfSearch = DateTime.Parse(EndDate);

            using (var db = new MyAnythingEntities())
            {
                var model = (from hotel in db.Hotel
                             join
                                 room in db.Room
                                 on hotel.ID equals room.HotelId
                             join order in db.OrderMaster
                             on room.ID equals order.ProductId
                             join pay in db.PayGo on order.MerchantOrderNo equals pay.MerchantOrderNo
                             where (order.Created >= BeginDateOfSearch && order.Created <= EndDateOfSearch) &&
                             (UserId <= 0 || hotel.UserId == UserId) &&
                             ((string.IsNullOrEmpty(search.Status) || search.Status.ToLower().Trim().Equals("none")) || order.Status == search.Status) &&
                             (string.IsNullOrEmpty(search.Keyword) || hotel.Name.Contains(search.Keyword) || room.Name.Contains(search.Keyword)) &&
                             (search.HotelId == 0 || search.HotelId == hotel.ID)
                             select new AccountingViewModel
                             {
                                 ID = order.ID,
                                 Paid = order.PayAmt,
                                 Bonus = order.BonusAmt,
                                 HotelName = hotel.Name,
                                 Income = order.PayAmt - order.BonusAmt,
                                 OrderDate = order.Created,
                                 OrderId = order.MerchantOrderNo,
                                 Quantity = order.Quantity,
                                 RoomName = room.Name,
                                 Status = order.Status,
                                 Amount = order.Amount,
                                 
                             }).ToList();

                MasterModel.Accounting = model;
                
                MasterModel.IncomeTotal = model.Sum(o => o.Income).ToString("#,##0");
                MasterModel.AmoutTotal = model.Sum(o => o.Amount).ToString("#,##0");
                var AccountingExcel = (from hotel in db.Hotel
                                       join
                                           room in db.Room
                                           on hotel.ID equals room.HotelId
                                       join order in db.OrderMaster
                                       on room.ID equals order.ProductId
                                       join pay in db.PayGo on order.MerchantOrderNo equals pay.MerchantOrderNo
                                       where (order.Created >= BeginDateOfSearch && order.Created <= EndDateOfSearch) &&
                                       (UserId <= 0 || hotel.UserId == UserId) &&
                                       ((string.IsNullOrEmpty(search.Status) || search.Status.ToLower().Trim().Equals("none")) || order.Status == search.Status) &&
                                       (string.IsNullOrEmpty(search.Keyword) || hotel.Name.Contains(search.Keyword) || room.Name.Contains(search.Keyword)) &&
                                       (search.HotelId == 0 || search.HotelId == hotel.ID)
                                       select new AccountingExcelModel
                                       {
                                           訂單編號 = order.MerchantOrderNo,
                                           訂單日期 = order.Created.ToString(),
                                           旅館 = hotel.Name,
                                           房型 = room.Name,
                                           住址 = hotel.Address,
                                           付款方式 = order.PaymentType,
                                           付款日期 = pay.PayTime.ToString(),
                                           付款狀態 = order.Status,                                
                                           消費使用紅利抵扣金額 = order.BonusAmt,
                                           消費金額 = order.Amount,
                                           業者實收金額 = pay.Amt,
                                           電話 = hotel.Tel,
                                           實付金額 = (order.Amount - order.BonusAmt),
                                           付款訊息 = pay.Message
                                       }).ToList();
                HttpContext.Current.Session["AccountingExcel"] = AccountingExcel;
                return MasterModel;
            }
        }

        
    }

    public class AccountingSearchModel
    {
        public string BeginDate { get; set; }
        public string EndDate { get; set; }

        public int HotelId { get; set; }

        public string Status { get; set; }

        public string Keyword { get; set; }

        public int UserId { get; set; }
    }

    public class AccountingExcelModel
    {
        //public int ID { get; set; }

        public string 訂單編號 { get; set; }
        public string 付款訊息 { get; set; }
        public string  旅館{ get; set; }
        public string 房型 { get; set; }
        public string 住址 { get; set; }
        public string 電話 { get; set; }
        //public string 開始日期 { get; set; }
        //public int Amount { get; set; }
        //public string 截至日期 { get; set; }

        public decimal 消費金額 { get; set; }
        public decimal 實付金額 { get; set; }

        public decimal 消費使用紅利抵扣金額 { get; set; }
        public decimal 業者實收金額 { get; set; }

        public string 付款狀態 { get; set; }
        public string 付款方式 { get; set; }

        public string 訂單日期 { get; set; }
        public string 付款日期 { get; set; }
        //public int HotelID { get; set; }

        public DataTable ConvertListToDataTable<T>(List<T> items)
        {
            // New table.
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}