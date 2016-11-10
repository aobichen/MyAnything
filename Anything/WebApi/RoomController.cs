using Anything.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Anything.WebApi
{
    public class CalendarEvent
    {
        public string Title { get; set; }
       
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        public string DayType { get; set; }

        public string DayText { get; set; }
    }

    public class CalendarPost
    {
        public DateTime date { get; set; }
        //public decimal price { get; set; }
        //public bool off { get; set; }
        public int RoomID { get; set; }
        public string daytype { get; set; }

        public string daytext { get; set; }
    }
    public class RoomController : ApiController
    {
        [HttpPost]
        [Route("GetRoomPrice/{id}")]
        public List<CalendarEvent> HotelImageUpload(int id)
        {
            var db = new MyAnythingEntities();
            var Now = DateTime.Now;
            var Begin = Now.AddDays(1);
            var End = Now.AddMonths(1);
            decimal Price = 0;
            List<CalendarEvent> Events = new List<CalendarEvent>();

            Price = 1000;
            

            Events = (from room in db.RoomPrice
                      where room.ROOMID == id
                      select new CalendarEvent
                      {
                          Start = room.Date,
                          End = room.Date,
                         DayText = room.DayText,
                         DayType = room.DayType
                      }).ToList();

            DateTime epoc = new DateTime(1970, 1, 1);
            List<CalendarEvent> events = new List<CalendarEvent>();
            if (Events == null || Events.Count <= 0)
            {
                for (var date = Begin; date < End; date = date.AddDays(1.0))
                {
                    var beginDay = DateTime.Parse(date.ToShortDateString());
                    var endDay = DateTime.Parse(date.ToShortDateString());
                    events.Add(new CalendarEvent
                    {
                        Title = "Event" + id.ToString(),
                        Start = beginDay,
                        End = endDay,
                        DayType = "0"
                    });
                }
            }
            else
            {
                for (var date = Begin; date < End; date = date.AddDays(1.0))
                {

                    var beginDay = DateTime.Parse(date.ToShortDateString());
                    var endDay = DateTime.Parse(date.ToShortDateString());
                    var Off = false;
                    decimal CurrentPrice = 0;
                    int Quantity = 0;
                    var DayType = "0";
                    var Current = Events.Where(o => o.Start == beginDay).FirstOrDefault();
                    if (Current != null)
                    {
                        DayType = Current.DayType;
                    }
                    else
                    {
                        DayType = "0";
                    }
                    events.Add(new CalendarEvent
                    {
                        Title = "Event" + id.ToString(),
                        Start = beginDay,
                        End = endDay,
                       DayType = DayType
                    });
                }
            }
            return events;
        }

        [HttpPost]
        [Route("RoomSave")]
        public HttpResponseMessage RoomPriceSave(List<CalendarPost> data)
        {
            var db = new MyAnythingEntities();
            HttpResponseMessage response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new { message = "ok" }))
            };

            if (data.Count <= 0)
            {
                return response;
            }

            var CreateModel = new List<RoomPrice>();

            try
            {
                foreach (var item in data)
                {
                    if (db.RoomPrice.Any(o => o.Date == item.date && o.ROOMID == item.RoomID))
                    {
                        var obj = db.RoomPrice.Where(o => o.Date == item.date && o.ROOMID == item.RoomID).FirstOrDefault();
                        obj.ROOMID = item.RoomID;
                        obj.Date = item.date;
                        obj.DayType = item.daytype;
                        obj.DayText = item.daytext;
                    }
                    else
                    {
                        db.RoomPrice.Add(new RoomPrice
                        {
                            DayText = item.daytext,
                            DayType = item.daytype,
                            Date = item.date,
                            ROOMID = item.RoomID
                        });
                    }
                    
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(JsonConvert.SerializeObject(new { message = ex.Message.ToString() }))
                };

                return response;
            }


            return response;
        }
    }
}
