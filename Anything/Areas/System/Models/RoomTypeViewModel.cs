using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Areas.System.Models
{
    public class RoomTypeViewModel
    {
        public int ID { get; set; }
        public string ItemDescription { get; set; }
        public string ItemType { get { return "Rooms"; } }
        public string Remark { get; set; }
        public string ItemCode { get; set; }
        public bool Enabled { get; set; }
        public string TypeText { get; set; }

        public List<RoomTypeViewModel> Query()
        {
            using (var db = new MyAnythingEntities())
            {
                var model = db.CodeFile.Where(o => o.ItemType == "Rooms").Select(o => new RoomTypeViewModel
                {
                    ID = o.ID,
                    ItemCode = o.ItemCode,
                    ItemDescription = o.ItemDescription

                }).ToList();
                return model;
            }
        }

        public RoomTypeViewModel Single(int id)
        {
            var model = new RoomTypeViewModel();
            using (var db = new MyAnythingEntities())
            {
                model = db.CodeFile.Where(o => o.ItemType == "Rooms" && o.ID == id).Select(o => new RoomTypeViewModel
                {
                    ID = o.ID,
                    ItemCode = o.ItemCode,
                    ItemDescription = o.ItemDescription

                }).FirstOrDefault();
            }
            return model;
        }

        public void Edit()
        {
            using (var db = new MyAnythingEntities())
            {
                if (ID > 0)
                {
                    var model = db.CodeFile.Where(o => o.ID == ID && o.ItemType == "Rooms").FirstOrDefault();
                    if (model != null)
                    {
                        model.ItemDescription = ItemDescription;
                        model.ItemType = ItemType;
                        model.Remark = model.Remark;
                        model.ItemCode = ItemCode;
                        model.Enabled = model.Enabled;
                        model.TypeText = model.TypeText;
                        db.SaveChanges();
                    }
                }
                else
                {
                    var model = new CodeFile();
                    model.ItemDescription = ItemDescription;
                    model.ItemType = "Rooms";
                    model.Remark = string.Empty;
                    model.ItemCode = ItemCode;
                    model.Enabled = true;
                    model.TypeText = "房型";
                    db.CodeFile.Add(model);
                    db.SaveChanges();
                }
            }

        }
    }
}