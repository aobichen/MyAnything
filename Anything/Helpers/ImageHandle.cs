using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Helpers
{
    public class ImageHandle
    {
        protected MyAnythingEntities _db;

       public ImageHandle()
            : base()
        {
            _db = new MyAnythingEntities();
        }

        public void CreateHotelImage(List<HotelImage> Images){
            _db.HotelImage.AddRange(Images);
            _db.SaveChanges();
        }

        public void CreateRoomImage(List<RoomImage> Images)
        {
            _db.RoomImage.AddRange(Images);
            _db.SaveChanges();
        }
    }
}