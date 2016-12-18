using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Areas.System.Models
{
    public class BedTypeModel
    {
         public int ID { get; set; }
        public string ItemDescription { get; set; }
        public string ItemType { get { return "Beds"; } }
        public string Remark { get; set; }
        public string ItemCode { get; set; }
        public bool Enabled { get; set; }
        public string TypeText { get; set; }

        public List<BedTypeModel> Query()
        {
            using (var db = new MyAnythingEntities())
            {
                var model = db.CodeFile.Where(o => o.ItemType == "Beds").Select(o => new BedTypeModel { ID = o.ID,
                    ItemCode = o.ItemCode,
                    ItemDescription = o.ItemDescription
                    
                }).ToList();
                return model;
            }
        }

        public BedTypeModel Single(int id)
        {
            var model = new BedTypeModel();
            using (var db = new MyAnythingEntities())
            {
                 model = db.CodeFile.Where(o => o.ItemType == "Beds" && o.ID == id).Select(o => new BedTypeModel
                {
                    ID = o.ID,
                    ItemCode = o.ItemCode,
                    ItemDescription = o.ItemDescription

                }).FirstOrDefault();
            }
            return model;
        }

        public void Edit(){
            using(var db = new  MyAnythingEntities())
            {
                if (ID > 0)
                {
                    var model = db.CodeFile.Where(o => o.ID == ID && o.ItemType == "Beds").FirstOrDefault();
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
                    model.ItemType = "Beds";
                    model.Remark = string.Empty;
                    model.ItemCode = ItemCode;
                    model.Enabled = true;
                    model.TypeText = "床型";
                    db.CodeFile.Add(model);
                    db.SaveChanges();
                }
            }
           
        }
    }
}