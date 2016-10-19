using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.ViewModels
{
    public class VIPModel
    {
        public string VIPDescription { get; set; }

        public string VIPPrice { get; set; }
        public string VIPSalePrice { get; set; }

        public string SaleDays { get; set; }
    }

    public class VIPOrderViewModel
    {        
        public int Vip { get; set; }
        public decimal Pay { get; set; }
        public string PayMethod { get; set; }
        public string OrderStatus { get; set; }
        public string PayType { get; set; }
        
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
    public class VIPsViewModel
    {
        public VIPsViewModel(int id)
        {
            Id = id;
            Days = GetDays();
        }
        public int Id{ get; set; }

        public System.DateTime BeginDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int Days { get; set; }
        public int HotelId { get; set; }

        public List<SelectListItem> Hotels { get { return GetHotels(); }}


        private int GetDays()
        {
            string d = string.Empty;
            var code = "VIPDays";
            using (var _db = new AnythingEntities())
            {
                var m = _db.SysManage.Where(o => o.FieldCode == code).FirstOrDefault();
                if (m != null)
                {
                    d = m.Value;
                }
            }
            return int.Parse(d);
        }

        private List<SelectListItem> GetHotels(){
            var selectlist = new List<SelectListItem>();
           
            using (var _db = new AnythingEntities())
            {
                var hotels = _db.Hotel.Where(o => o.UserId == Id).ToList();
                foreach (var h in hotels)
                {
                    selectlist.Add(new SelectListItem()
                    {
                        Text = h.Name,
                        Value = h.ID.ToString(),
                        Selected = h.ID.Equals(hotels.First().ID)
                    });
                }
            }
            return selectlist;
        }

  
    }
}