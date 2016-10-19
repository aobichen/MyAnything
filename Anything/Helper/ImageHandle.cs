using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Helper
{
    public class ImageHandle
    {
       protected AnythingEntities _db;

       public ImageHandle()
            : base()
        {
            _db = new AnythingEntities();
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