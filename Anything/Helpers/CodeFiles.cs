using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Helpers
{
    public class CodeFiles
    {
        private MyAnythingEntities _db;
        public CodeFiles(){
            _db = new MyAnythingEntities();
        }

        public SelectList GetRoomsSelectList(int selected=0)
        {

            var Beds = GetRooms();
            SelectList SelectList = new SelectList(Beds, "ID", "ItemCode", selected);
            return SelectList;

        }
        public List<CodeFile> GetRooms()
        {
            var ItemType = "Rooms";
            return GetCodeFiles(ItemType);

        }
        public SelectList GetBedsSelectList(int selected=0)
        {
         
            var Beds = GetBeds();
            SelectList SelectList = new SelectList(Beds, "ID", "ItemCode", selected);
            return SelectList;

        }
        public List<CodeFile> GetBeds(){
            var ItemType = "Beds";          
            return GetCodeFiles(ItemType);

        }

        private List<CodeFile> GetCodeFiles(string ItemType)
        {
            var Items = _db.CodeFile.Where(o => o.ItemType == ItemType).ToList();
            return Items;
        }

        
    }
}