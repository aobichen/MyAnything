using Anything.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Areas.System.Models
{
    public class BonusNoticeViewModel
    {
        public string ItemCode { get; set; }
        public string ItemValue { get; set; }

       [UIHint("tinymce_jquery_full"), AllowHtml]
        public string ItemDescription { get; set; }
        public string ItemUnit { get; set; }
        public string ItemType { get; set; }

        public int Modify { get; set; }
        public BonusNoticeViewModel Query()
        {
            var ItemType = "BonusNoticeFor25date";
            using(var db = new MyAnythingEntities()){
                var model = db.SystemField.Where(o => o.ItemType == ItemType).Select(o => new BonusNoticeViewModel {
                    ItemDescription = o.ItemDescription
                }).FirstOrDefault();
                return model;
            }

            //return string.Empty;
            
        }

        public void Edit()
        {
            var ItemType = "BonusNoticeFor25date";
            using (var db = new MyAnythingEntities())
            {
                var model = db.SystemField.Where(o => o.ItemType == ItemType).FirstOrDefault();
                model.ItemDescription = ItemDescription;
                model.Modified = DateTime.Now;
                model.Modify = Modify;
                db.SaveChanges();
            }
        }
    }
}