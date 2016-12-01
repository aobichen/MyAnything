using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;


namespace Anything.ViewModels
{
    public class BounsViewModel
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> PayTime { get; set; }
        public Nullable<decimal> OrderAmt { get; set; }
        public decimal Bouns { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Status { get; set; }
        public decimal AmtMinLimit { get; set; }
        public Nullable<int> UseMonth { get; set; }

        public string MerchantOrderNo { get; set; }

        public void Create()
        {
            using (var db = new MyAnythingEntities())
            {
                var Bouns = db.SystemField.Where(o => o.ItemCode == "BS").ToList();
                var CarshFlow = Bouns.Where(o => o.ItemType == "CarshFlow").FirstOrDefault();
                double Carsh = 0;
                if (CarshFlow != null)
                {
                    Carsh = double.Parse(CarshFlow.ItemValue);
                }

                var Platforms = Bouns.Where(o => o.ItemType == "Platform").FirstOrDefault();
                double Platform = 0;
                if (Platforms != null)
                {
                    Platform = double.Parse(Platforms.ItemValue);
                }

                var HotelPromo = Bouns.Where(o => o.ItemType == "HotelPromo").FirstOrDefault();
                double Hotel = 0;
                if (HotelPromo != null)
                {
                    Hotel = double.Parse(HotelPromo.ItemValue);
                }

                var BuyFeedback = Bouns.Where(o => o.ItemType == "BuyFeedback").FirstOrDefault();
                double Buy = 0;
                if (BuyFeedback != null)
                {
                    Buy = double.Parse(BuyFeedback.ItemValue);
                }

                var UpperUser = Bouns.Where(o => o.ItemType == "UpperUser").FirstOrDefault();
                double Upper = 0;
                if (UpperUser != null)
                {
                    Upper = double.Parse(UpperUser.ItemValue);
                }

                var AmtMinLimit = Bouns.Where(o => o.ItemType == "AmtMinLimit").FirstOrDefault();
                double MinAmt = 0;
                if (AmtMinLimit != null)
                {
                    MinAmt = double.Parse(AmtMinLimit.ItemValue);
                }

                var CarshAmt = (double)OrderAmt * (Carsh * 0.01);
                var PlatformAmt = (double)OrderAmt * (Platform * 0.01);
                var HotelAmt = (double)OrderAmt * (Hotel * 0.01);
                var BuyAmt = (double)OrderAmt * (Buy * 0.01);
                var UpperAmt = (double)OrderAmt * (Upper * 0.01);

                

                var Today = DateTime.Now;
               
               
                db.MyBouns.Add(new MyBouns
                {
                    AmtMinLimit = (decimal)MinAmt,
                    Bouns = (decimal)BuyAmt,
                    Created = Today,
                    OrderAmt = OrderAmt.Value,
                    OrderID = OrderID.Value,
                    PayTime = PayTime.Value,
                    Status = Status,
                    UseMonth = UseMonth.Value,
                    UserID = UserID.Value,
                    MerchantOrderNo = MerchantOrderNo,
                    ParentID =0
                });

               
                var UpperUsers = new UpperUserModel().GETUpperUserList(UserID.Value);
                var avgAmt = UpperAmt / UpperUsers.Count;
                foreach (var item in UpperUsers)
                {
                    db.MyBouns.Add(new MyBouns
                    {
                        AmtMinLimit = (decimal)MinAmt,
                        Bouns = (decimal)avgAmt,
                        Created = Today,
                        OrderAmt = OrderAmt.Value,
                        OrderID = OrderID.Value,
                        PayTime = PayTime.Value,
                        Status = Status,
                        UseMonth = UseMonth.Value,
                        UserID = item.ID,
                        MerchantOrderNo = MerchantOrderNo,
                        ParentID = 0
                    });
                }

                db.SaveChanges();
                
            }
        }
    }

    public class UpperUsers
    {
        public int ID { get; set; }
        public string Email { get; set; }

        
    }

    public class UpperUserModel
    {
        
        public List<UpperUsers> Users { get; set; }

        public UpperUserModel()
        {
            if (Users == null)
            {
                Users = new List<UpperUsers>();
            }
        }
        public List<UpperUsers> GETUpperUserList(int ID)
        {
            using (var db = new ApplicationDbContext())
            {
                var User = db.Users.Where(o => o.Id == ID).FirstOrDefault();
                if (User != null && User.Id != 0 && Users.Count<6)
                {
                    var RecomandUser = db.Users.Where(o => o.UserCode == User.Recommend).FirstOrDefault();
                    if (RecomandUser != null)
                    {
                        Users.Add(new UpperUsers { ID=RecomandUser.Id,Email = RecomandUser.Email });
                        GETUpperUserList(RecomandUser.Id);
                    }
                }
            }

            return Users;
        }
    }

    public class BounsForSysModel
    {
        public string CarshFlow { get; set; }
        public string Platform { get; set; }
        public string HotelPromo { get; set; }
        public string BuyFeedback { get; set; }
        public string UpperUser { get; set; }
       
        public string AmtMinLimit { get; set; }

        public BounsForSysModel Query()
        {
            var Result = new BounsForSysModel();
            using (var db = new MyAnythingEntities())
            {
                var model = db.SystemField.Where(o => o.ItemCode == "BS").ToList();
                var CarshFlowModel = model.Where(o => o.ItemType == "CarshFlow").FirstOrDefault().ItemValue;
                Result.CarshFlow = CarshFlowModel;
                var PlatformModel = model.Where(o => o.ItemType == "Platform").FirstOrDefault().ItemValue;
                Result.Platform = PlatformModel;
                var HotelPromoModel = model.Where(o => o.ItemType == "HotelPromo").FirstOrDefault().ItemValue;
                Result.HotelPromo = HotelPromoModel;
                var BuyFeedbackModel = model.Where(o => o.ItemType == "BuyFeedback").FirstOrDefault().ItemValue;
                Result.BuyFeedback = BuyFeedbackModel;
                var UpperUserModel = model.Where(o => o.ItemType == "UpperUser").FirstOrDefault().ItemValue;
                Result.UpperUser = UpperUserModel;
                var AmtMinLimitModel = model.Where(o => o.ItemType == "AmtMinLimit").FirstOrDefault().ItemValue;
                Result.AmtMinLimit = AmtMinLimitModel;
            }
            return Result;
        }

        public void Edit()
        {
            using (var db = new MyAnythingEntities())
            {
                var model = db.SystemField.Where(o => o.ItemCode == "BS").ToList();
                var CarshFlowModel = model.Where(o => o.ItemType == "CarshFlow").FirstOrDefault();
                CarshFlowModel.ItemValue =  CarshFlow;
                var PlatformModel = model.Where(o => o.ItemType == "Platform").FirstOrDefault();
                PlatformModel.ItemValue = Platform;
                var HotelPromoModel = model.Where(o => o.ItemType == "HotelPromo").FirstOrDefault();
                HotelPromoModel.ItemValue = HotelPromo;
                var BuyFeedbackModel = model.Where(o => o.ItemType == "BuyFeedback").FirstOrDefault();
                BuyFeedbackModel.ItemValue = BuyFeedback;
                var UpperUserModel = model.Where(o => o.ItemType == "UpperUser").FirstOrDefault();
                UpperUserModel.ItemValue = UpperUser;
                var AmtMinLimitModel = model.Where(o => o.ItemType == "AmtMinLimit").FirstOrDefault();
                AmtMinLimitModel.ItemValue = AmtMinLimit;
               
                db.SaveChanges();
            }
        }
    }
    
}