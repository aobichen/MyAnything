using Anything.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Anything.Areas.System.Models
{
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

        

        public List<AccountingViewModel> Query(AccountingSearchModel search = null)
        {
            var BeginDate = (search == null || search.BeginDate <= DateTime.MinValue) ? DateTime.Now : search.BeginDate;
            var EndDate = (search==null || search.EndDate <= DateTime.MinValue) ? DateTime.Now : search.EndDate;

            using (var db = new MyAnythingEntities())
            {
                var model = (from hotel in db.Hotel
                             join
                                 room in db.Room
                                 on hotel.ID equals room.HotelId
                             join order in db.OrderMaster
                             on room.ID equals order.ProductId
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
                                 Amount = order.Amount
                             }).ToList();
                return model;
            }
        }


    }

    public class AccountingSearchModel
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public int HotelId { get; set; }

        public string Status { get; set; }
    }
}