using Anything.Helpers;
using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Areas.My.Models
{
    public class MyOrderModel
    {
        public int ID { get; set; }
        public string HotelName { get; set; }
        public string RoomName { get; set; }

        public List<int> RoomID { get; set; }

        public string MerchantOrderNo { get; set; }

        public int Quantity { get; set; }
        public decimal Amt { get; set; }
        public decimal OrderAmt { get; set; }
        public decimal Bonus { get; set; }
        public string PaymentType { get; set; }
        public string PayStatus { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        //public string OrderId { get; set; }

        public int UserId { get; set; }


        public DateTime Created { get; set; }

        public string SearchKey { get; set; }

        public List<MyOrderModel> List()
        {

            var Today = DateTime.Now;
            var model = new List<MyOrderModel>();
            using (var db = new MyAnythingEntities())
            {
                model = (from order in db.OrderMaster
                         join room in db.Room on order.ProductId equals room.ID
                         join pay in db.PayGo on order.MerchantOrderNo equals pay.MerchantOrderNo into paygo
                         from x in paygo.DefaultIfEmpty()
                         where order.UserId == UserId
                         && (string.IsNullOrEmpty(SearchKey) ||
                         (order.Name.Contains(SearchKey) || order.Tel.Contains(SearchKey) || order.MerchantOrderNo.Contains(SearchKey)))
                         select new MyOrderModel
                         {
                             CheckInDate = order.CheckIn,
                             CheckOutDate = order.CheckOut,
                             ID = order.ID,
                             MerchantOrderNo = order.MerchantOrderNo,
                             PaymentType = order.PaymentType,
                             PayStatus = (order.ExpireDate != null && order.Status == OrderType.Unpaid.ToString() && DateTime.Compare(order.ExpireDate.Value, Today) < 0) ? "超過繳款期限" :
                                         (order.Status == OrderType.Paid.ToString() ? "付款完成" : (order.Status == OrderType.Expired.ToString() ? "超過繳款期限" : "未付款")),
                             RoomName = order.ProductName,
                             Created = order.Created
                         }).ToList();

            }
            return model;
        }
    }
    public class HotelOrderModel
    {
        public int ID { get; set; }
        public string RoomName { get; set; }

        public List<int> RoomID { get; set; }

        public int Quantity {get;set;}
        public decimal Amt {get;set;}
        public decimal Bonus {get;set;}
        public string MerchantOrderNo { get; set; }

        public string PaymentType { get; set; }
        public string PayStatus { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        //public string OrderId { get; set; }
        public string OrderName { get; set; }

        public decimal OrderAmt { get; set; }

        public string OrderTel { get; set; }

        public DateTime Created { get; set; }

        public string SearchKey { get; set; }

        public List<HotelOrderModel> List()
        {
                       
            var Today = DateTime.Now;
            var model = new List<HotelOrderModel>();
            using(var db = new MyAnythingEntities()){
                 model = (from order in db.OrderMaster
                             join room in db.Room on order.ProductId equals room.ID
                             join pay in db.PayGo on order.MerchantOrderNo equals pay.MerchantOrderNo into paygo
                             from x in paygo.DefaultIfEmpty()
                             where RoomID.Contains(room.ID)
                             && (string.IsNullOrEmpty(SearchKey) || 
                             (order.Name.Contains(SearchKey) || order.Tel.Contains(SearchKey)||order.MerchantOrderNo.Contains(SearchKey)))
                             select new HotelOrderModel
                             {
                                 CheckInDate = order.CheckIn,
                                 CheckOutDate = order.CheckOut,
                                 ID = order.ID,
                                 MerchantOrderNo = order.MerchantOrderNo,
                                 OrderTel = order.Tel,
                                 OrderName = order.Name,
                                 PaymentType = order.PaymentType,
                                 PayStatus = (order.ExpireDate !=null &&  order.Status == OrderType.Unpaid.ToString() && DateTime.Compare(order.ExpireDate.Value, Today)<0)? "超過繳款期限" :
                                             (order.Status == OrderType.Paid.ToString() ? "付款完成" : (order.Status == OrderType.Expired.ToString() ? "超過繳款期限" : "未付款")),
                                 RoomName = order.ProductName,
                                 Created = order.Created,
                                  Quantity = order.Quantity,
                                   Amt = order.PayAmt,
                                   Bonus = order.BonusAmt,
                                   OrderAmt = order.Amount
                             }).ToList();



            }
            return model;
        }

        

        public string PaymentStatus(OrderMaster order)
        {
            var Today = DateTime.Now;
            var value = string.Empty;
            if (order.PaymentType != "Credit" && (order.ExpireDate.Value.Year > Today.Year &&
                                 order.ExpireDate.Value.Month >= Today.Month && order.ExpireDate.Value.Date >= Today.Date))
            {
                value = "繳費過期";
            }
            else
            {
                switch (order.PaymentType)
                {
                    case "Unpaid":
                        value = "未付款";
                        break;
                    case "Paid":
                        value = "已付款";
                        break;
                  
                }
            }
            return value;
        }

    }
}