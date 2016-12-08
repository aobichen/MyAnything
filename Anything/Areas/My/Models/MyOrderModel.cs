using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Areas.My.Models
{
    public class MyOrderModel
    {
    }

    public class HotelOrderModel
    {
        public int ID { get; set; }
        public string RoomName { get; set; }


        public string PaymentType { get; set; }
        public string PayStatus { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }

        public string OrderId { get; set; }
        public string OrderName { get; set; }

        public string OrderTel { get; set; }

    }
}