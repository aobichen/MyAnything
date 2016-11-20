using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.ViewModels
{
    public class OrderViewModel
    {
        public string RoomName { get; set; }
        public string HotelName { get; set; }
       
        public int RoomId { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
        public System.DateTime CheckIn { get; set; }
        public System.DateTime CheckOut { get; set; }

        //public Decimal Total { get; set; }

        public string MerchantTradeNo { get; set; }
        //public string PayMethod { get; set; }
        //public string PayStatus { get; set; }
        //public string Status { get; set; }
        //public int Bonus { get; set; }
        //public string PayName { get; set; }
        //訂購人資料
        public string Name { get; set; }
       // public string Tel { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        //public string PayNumber { get; set; }
        public System.DateTime Payed { get; set; }
        //public System.DateTime Created { get; set; }
        public string Remark { get; set; }

        public string Key { get; set; }

        public Anything.Models.PayGo PayGo { get; set; }
        //public System.DateTime Modified { get; set; }
        //public int Modify { get; set; }
        //public string PayVendor { get; set; }

        private enum PayMethod
        {
            Credit=0,
            ATM,
            CVS
        }
    }

    public class MyOrderOfHotel
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int ID { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public decimal Total { get; set; }
        public decimal Price { get; set; }
        public int  Amount { get; set; }
        public string MerchantTradeNo { get; set; }

        public string PayStatus { get; set; }

        public DateTime Created { get; set; }
    }
    

    public class SelectedAmount
    {
        public string key { get; set; }
        public int RoomId { get; set; }
        public int Amount { get; set; }
    }
}