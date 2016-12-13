using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Areas.My.Models
{
    public class MyBonusSearchModel
    {
        public int Status { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class MyBonusModel
    {
        public int ID { get; set; }

        public int UserId { get; set; }
        public decimal Amt { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        
    }
}