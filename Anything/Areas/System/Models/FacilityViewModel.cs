using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Areas.System.Models
{
    public class FacilityViewModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool Show { get; set; }
        public DateTime Created { get; set; }


        public void Edit()
        {
            using (var db = new MyAnythingEntities())
            {
                if (ID <= 0)
                {
                    db.Facility.Add(new Facility
                    {
                        Text = Text,
                        Show = Show,
                        Enabled = Enabled,
                        Description = Description,
                        Created = DateTime.Now
                    });
                }
                else
                {
                   var model =  db.Facility.Find(ID);
                   model.Description = Description;
                   model.Enabled = Enabled;
                   model.Show = Show;
                   model.Text = Text;
                }
                
                db.SaveChanges();
            }
        }

        public List<FacilityViewModel> Query()
        {
            var model = new List<FacilityViewModel>();
            using (var db = new MyAnythingEntities())
            {
                 model = db.Facility.Select(o => new FacilityViewModel
                {
                    Created = o.Created.Value,
                    Description = o.Description,
                    Enabled = o.Enabled,
                    ID = o.ID,
                    Show = o.Show.Value,
                    Text = o.Text
                }).ToList();
            }
            return model;
        }
    }
}