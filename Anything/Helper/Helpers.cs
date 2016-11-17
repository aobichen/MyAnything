using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Helper
{
    public class Helpers
    {
       // private static AnythingEntities _db;
        public Helpers()
        {
           
        }

        public class Member
        {
            static ApplicationDbContext _db = new ApplicationDbContext();
            public static string Email(int id)
            {
                var u = _db.Users.Where(o => o.Id == id).FirstOrDefault();
                return _db.Users.Where(o => o.Id == id).FirstOrDefault().Email;
            }
        }
       
        public class Hotel
        {
            public static bool HasWifi(string options)
            {
                var item = "1";
                        if (string.IsNullOrEmpty(options))
                        {
                            return false;
                        }
                        var items = options.Split(',').ToList();
                        if (items.Count <= 0)
                        {
                            return false;
                        }
                        return items.Any(o => o == item);
                     
            }

            public static bool HasCarPark(string options)
            {
                var item = "2";
                if (string.IsNullOrEmpty(options))
                {
                    return false;
                }
                var items = options.Split(',').ToList();
                if (items.Count <= 0)
                {
                    return false;
                }
                return items.Any(o => o == item);

            }
        }

        public class Room
        {                      
            public static string RoomType(string options)
            {
                var _db = new MyAnythingEntities();
               
                var op = options.Split(',');
                var int_room = int.Parse(op[0]);
                var room = _db.CodeFile.Where(o => o.ID == int_room).FirstOrDefault().ItemCode;
                var int_bed = int.Parse(op[1]);
                var bed = _db.CodeFile.Where(o => o.ID == int_bed).FirstOrDefault().ItemCode;
                var amount = int.Parse(op[2]);
                _db.Dispose();
                return string.Format("{0}【{1}X{2}】", room, bed, amount);

            }

            public static string GetRoomForID(int ID)
            {
                var _db = new MyAnythingEntities();

                var Room = _db.Room.Find(ID);
                var Name = Room != null ? Room.Name : string.Empty;
                return Name;

            }

            public static string GetHotelForID(int ID)
            {
                var _db = new MyAnythingEntities();

                var Room = _db.Room.Find(ID);
                var Name = Room != null ? Room.Hotel.Name : string.Empty;
                return Name;

            }

           
        }

        public class CityArea
        {
            //var _db = new AnythingEntities();
            public static string GetCityName(int id)
            {
                var _db = new MyAnythingEntities();
                var city = _db.City_TW.Find(id).City;
                return city;
            }

            public static string GetCityArea(int city, int area)
            {
                var _db = new MyAnythingEntities();
                var c = _db.City_TW.Find(city).City;
                var a = _db.Area_TW.Find(area).Area;
                return c + a;
            }
        }
    }
}