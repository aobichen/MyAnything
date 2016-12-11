using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.ViewModels
{
    public class FacilityModel
    {
        public List<SelectListItem> SelectListItems { get; set; }

        public List<string> SelectedItems { get; set; }
        public MyAnythingEntities _db;
        public FacilityModel()
        {
            if (_db == null)
            {
                _db = new MyAnythingEntities();
            }

            Facility();
        }

        public void Facility()
        {
            var Items = _db.Facility.ToList();
            
            if (SelectListItems == null)
            {
                SelectListItems = new List<SelectListItem>();
            }
            //var SelectList = new List<SelectListItem>();
            //var SelectedItems = SelectedItems;
            foreach (var item in Items)
            {
                SelectListItems.Add(new SelectListItem
                { 
                    Text = item.Text,
                    Value = item.ID.ToString(),
                    Selected = SelectedItems == null ? false :
                    (SelectedItems.Contains(item.ID.ToString())?true:false)                    
                });
            }

            //return SelectList;
        }

        public List<SelectListItem> List(List<string> SelectedItems = null)
        {
            var Items = _db.Facility.ToList();
            
            if (SelectListItems == null)
            {
                SelectListItems = new List<SelectListItem>();
            }
            //var SelectList = new List<SelectListItem>();
            //var SelectedItems = SelectedItems;
            foreach (var item in Items)
            {
                SelectListItems.Add(new SelectListItem
                { 
                    Text = item.Text,
                    Value = item.ID.ToString(),
                    Selected = SelectedItems == null ? false :
                    (SelectedItems.Contains(item.ID.ToString())?true:false)                    
                });
            }

            return SelectListItems;
        }
    }
}