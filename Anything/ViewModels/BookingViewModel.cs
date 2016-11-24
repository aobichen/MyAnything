﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Anything.ViewModels
{
    public class BookingModel
    {
        public BookingInfo info { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
       
     
        public decimal Total { get; set; }
        public string RoomType { get; set; }
        public string BedType { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckInDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckOutDate { get; set; }

        public string Address { get; set; }

        public string Tel { get; set; }

        public string Email { get; set; }

        public int People { get; set; }

        public int MaxPeople { get; set; }

        public string DateList { get; set; }

        public int PaymentType { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }

    public class BookingInfo
    {
        public string Name
        {
            get;
            set;
        }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string Remark { get; set; }
    }

    public class BookingCommit
    {
        public BookingModel Booking { get; set; }
        public PayGoRequest PayGoRequest { get; set; }
    }
}