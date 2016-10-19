using Anything.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.ViewModels
{
    public class Locations
    {
        public int ID { get; set; }
        public string Location { get; set; }

        public string LocationText(int id)
        {
            
            var Locations = new Caches().TWLocation;
            var text = Locations.Where(o => o.ID == id).FirstOrDefault();
            if (text == null)
            {
                return string.Empty;
            }
            return text.Location;
        }
    }
}