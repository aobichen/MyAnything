﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Anything.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyAnythingEntities : DbContext
    {
        public MyAnythingEntities()
            : base("name=MyAnythingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CodeFile> CodeFile { get; set; }
        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<HotelImage> HotelImage { get; set; }
        public virtual DbSet<ImageStore> ImageStore { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<RoomImage> RoomImage { get; set; }
        public virtual DbSet<RoomPrice> RoomPrice { get; set; }
        public virtual DbSet<Scenic> Scenic { get; set; }
        public virtual DbSet<TEST> TEST { get; set; }
        public virtual DbSet<Hotel> Hotel { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Accounting> Accounting { get; set; }
        public virtual DbSet<SystemField> SystemField { get; set; }
        public virtual DbSet<OrderMaster> OrderMaster { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }
        public virtual DbSet<MyBonus> MyBonus { get; set; }
        public virtual DbSet<PayGo> PayGo { get; set; }
    }
}
