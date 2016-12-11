using Anything.Helpers;
using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class RoomAmt
    {
        public decimal CurrentAmt (int RoomId){
            var _db = new MyAnythingEntities();
            decimal amt = 0;
            DateTime BeginDate = DateTime.MinValue;
            DateTime EndDate = DateTime.MinValue;
            var Now = DateTime.Now;
            var Current = HttpContext.Current ;
           HttpContext.Current.Session["CheckInDate"] = Now.AddDays(1);
           HttpContext.Current.Session["CheckOutDate"] = Now.AddDays(2);
           if (Current.Session["CheckInDate"] == null || Current.Session["CheckOutDate"] == null)
            {
                BeginDate = Now.AddDays(1);
                EndDate = Now.AddDays(2);
            }
           else
           {
               BeginDate = DateTime.Parse(Current.Session["CheckInDate"].ToString());
               EndDate = DateTime.Parse(Current.Session["CheckOutDate"].ToString());
           }

           var day = ((int)BeginDate.DayOfWeek).ToString();
           if (day == "5" || day == "6")
           {
               var Room = _db.Room.Find(RoomId);
               var RoomAmt = _db.RoomPrice.Where(o => (o.Date.Year == BeginDate.Year && o.Date.Month == BeginDate.Month && o.Date.Day == BeginDate.Day)
                            && o.ROOMID == Room.ID).FirstOrDefault();
               if (RoomAmt == null)
               {
                   return Room.HolidayPrice;
               }
               else
               {
                   var PriceType = 99;
                   int.TryParse(RoomAmt.DayType, out PriceType);
                   amt = AmtFromRoomPrice(PriceType,Room);
               }
           }
           else
           {
               var Room = _db.Room.Find(RoomId);
               var RoomAmt = _db.RoomPrice.Where(o => (o.Date.Year == BeginDate.Year && o.Date.Month == BeginDate.Month && o.Date.Day == BeginDate.Day)
                            && o.ROOMID == Room.ID).FirstOrDefault();
               if (RoomAmt == null)
               {
                   return Room.DayPrice;
               }
               else
               {
                   var PriceType = 99;
                   int.TryParse(RoomAmt.DayType, out PriceType);
                   amt = AmtFromRoomPrice(PriceType, Room);
               }
           }
            return amt;
        }

        private decimal AmtFromRoomPrice(int type, Room room)
        {
            decimal amt = 0;
            switch (type)
            {
                case (int)RoomPriceType.Day:
                    amt = room.DayPrice;
                    break;
                case (int)RoomPriceType.Holiday:
                    amt = room.HolidayPrice;
                    break;
                case (int)RoomPriceType.Fixed:
                    amt = room.FixedPrice;
                    break;
            }
            return amt;
        }
    }


