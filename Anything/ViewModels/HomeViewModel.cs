using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.ViewModels
{
    public class HomeSearchViewModel
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<string> Facilities { get; set; }

        public List<string> Scenic { get; set; }

        
        public int Word { get; set; }
    }

    
}