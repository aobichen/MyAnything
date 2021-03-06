﻿using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;
using Anything.ViewModels;

namespace Anything.Helpers
{
    public class Caches
    {
        protected MyAnythingEntities _db;
        protected ObjectCache Cache;
       public Caches():base(){
           _db = new MyAnythingEntities();
           Cache = MemoryCache.Default;
        }

        public List<City> TWCity { get { return getCity();}}

        public List<Area> TWArea { get { return getArea(); } }

        public List<Locations> TWLocation { get { return getLocation(); } }

        private List<City> getCity()
       {
           var name = CacheName.TWCity.ToString();
           var City = Cache[name] as List<City>;
           if (City == null)
           {
               var c = _db.City.ToList();              
               City = c;
               Cache[name] = City;
           }

           return City;
       }

        private List<Area> getArea()
       {
           var name = CacheName.TWArea.ToString();
           var Area = Cache[name] as List<Area>;
           if (Area == null)
           {
               var c = _db.Area.ToList();
               Area = c;
               Cache[name] = Area;
           }

           return Area;
       }

     private List<Locations> getLocation(){
         var Locations = new List<Locations>();

         Locations.Add(new Locations() { ID = 1, Location = "北部" });
         Locations.Add(new Locations() { ID = 2, Location = "中部" });
         Locations.Add(new Locations() { ID = 3, Location = "南部" });
         Locations.Add(new Locations() { ID = 4, Location = "花東" });
         Locations.Add(new Locations() { ID = 5, Location = "外島" });
         return Locations;
     }

     

        public enum CacheName
        {
            None=0,
            TWCity,
            TWArea
        }
    }
}