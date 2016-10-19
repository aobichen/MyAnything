using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.ViewModels
{
    public class RoomCreateViewModel
    {
        public int ID { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public decimal SellPrice { get; set; }
        public Nullable<decimal> DiscountPrice { get; set; }
        public decimal Bonus { get; set; }
        public int Amount { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Information { get; set; }
        public bool Enabled { get; set; }

        public DateTime Created { get; set; }

        public int Creator { get; set; }
        public DateTime Modified { get; set; }
        public string SessionKey { get; set; }

        public string Person { get; set; }
        public int Beds { get; set; }
        public string BedType { get; set; }
        public string RoomBed { get; set; }

        
    }
}