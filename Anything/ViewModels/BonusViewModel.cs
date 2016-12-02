using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;


namespace Anything.ViewModels
{
    public class BonusViewModel
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> PayTime { get; set; }
        public Nullable<decimal> OrderAmt { get; set; }
        public decimal Bonus { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Status { get; set; }
        public int AmtMinLimit { get; set; }
        public string BonusStatus { get; set; }

        public string BonusType { get; set; }

        public Nullable<int> UseMonth { get; set; }

        public string MerchantOrderNo { get; set; }

        public void Create()
        {
            using (var db = new MyAnythingEntities())
            {
                var Bonus = db.SystemField.Where(o => o.ItemCode == "BS").ToList();
                var TotalFee = Bonus.Where(o => o.ItemType == "TotalFee").FirstOrDefault();
                double Total = 0;
                if (TotalFee != null)
                {
                    Total = double.Parse(TotalFee.ItemValue);
                }

                var BuyFee = Bonus.Where(o => o.ItemType == "BuyFee").FirstOrDefault();
                double Buy = 0;
                if (BuyFee != null)
                {
                    Buy = double.Parse(BuyFee.ItemValue);
                }

                

                var HotelPromo = Bonus.Where(o => o.ItemType == "HotelPromoFee").FirstOrDefault();
                double Hotel = 0;
                if (HotelPromo != null)
                {
                    Hotel = double.Parse(HotelPromo.ItemValue);
                }

                var UpperUser = Bonus.Where(o => o.ItemType == "UpperUser").FirstOrDefault();
                double Upper = 0;
                if (UpperUser != null)
                {
                    Upper = double.Parse(UpperUser.ItemValue);
                }

                var AmtMinLimit = Bonus.Where(o => o.ItemType == "MinLimitAmt").FirstOrDefault();
                double MinAmt = 0;
                if (AmtMinLimit != null)
                {
                    MinAmt = double.Parse(AmtMinLimit.ItemValue);
                }

                var TotalAmt = (double)OrderAmt * (Total * 0.01);
                var BuyAmt = TotalAmt * (TotalAmt * 0.01);
                var HotelAmt = (double)OrderAmt * (Hotel * 0.01);

                var UpperAmt = ((double)OrderAmt * (Upper * 0.01))/6;

                var LimitAmt = MinAmt;
                

                var Today = DateTime.Now;
               
               
                db.MyBonus.Add(new MyBonus
                {
                    AmtMinLimit = (int)MinAmt,
                    Bonus = (decimal)BuyAmt,
                    Created = Today,
                    OrderAmt = OrderAmt.Value,
                    OrderID = OrderID.Value,
                    PayTime = PayTime.Value,
                    PayStatus = Status,
                    UseMonth = UseMonth.Value,
                    UserID = UserID.Value,
                    MerchantOrderNo = MerchantOrderNo,
                    ParentID =0,
                    BonusType = BonusTypeEnum.Purchasing.ToString(),
                    BonusStatus = BonusStatusEnum.CurrentBonus.ToString(),
                    Notified = false
                });

               
                var UpperUsers = new UpperUserModel().GETUpperUserList(UserID.Value);
                var avgAmt = UpperAmt;
                foreach (var item in UpperUsers)
                {
                    db.MyBonus.Add(new MyBonus
                    {
                        AmtMinLimit = (int)MinAmt,
                        Bonus = (decimal)avgAmt,
                        Created = Today,
                        OrderAmt = OrderAmt.Value,
                        OrderID = OrderID.Value,
                        PayTime = PayTime.Value,
                        PayStatus = Status,
                        UseMonth = UseMonth.Value,
                        UserID = item.ID,
                        MerchantOrderNo = MerchantOrderNo,
                        ParentID = 0,
                        BonusType = BonusTypeEnum.Recommend.ToString(),
                        BonusStatus = BonusStatusEnum.CurrentBonus.ToString(),
                        Notified = false
                    });
                }

                db.SaveChanges();
                
            }
        }
    }


    public enum BonusTypeEnum
    {
        None =0,
        Purchasing,
        Recommend 
    }

    public enum BonusStatusEnum
    {
        None = 0,
        CurrentBonus,
        CanUse,
        NoOverAmount
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

    public class BonusForSysModel
    {
        public string TotalFee { get; set; }
        public string BuyFee { get; set; }
        public string UpperFee { get; set; }
        public string HotelPromoFee { get; set; }
       

        public string MinLimitAmt { get; set; }

        public BonusForSysModel Query()
        {
            var Result = new BonusForSysModel();
            using (var db = new MyAnythingEntities())
            {
                var model = db.SystemField.Where(o => o.ItemCode == "BS").ToList();
                var TotalFeeModel = model.Where(o => o.ItemType == "TotalFee").FirstOrDefault().ItemValue;
                Result.TotalFee = TotalFeeModel;
                var BuyFeeModel = model.Where(o => o.ItemType == "BuyFee").FirstOrDefault().ItemValue;
                Result.BuyFee = BuyFeeModel;
                var UpperFeeModel = model.Where(o => o.ItemType == "UpperFee").FirstOrDefault().ItemValue;
                Result.UpperFee = UpperFeeModel;
                var HotelPromoFeeModel = model.Where(o => o.ItemType == "HotelPromoFee").FirstOrDefault().ItemValue;
                Result.HotelPromoFee = HotelPromoFeeModel;
                var MinLimitAmtModel = model.Where(o => o.ItemType == "MinLimitAmt").FirstOrDefault().ItemValue;
                Result.MinLimitAmt = MinLimitAmtModel;
               
            }
            return Result;
        }

        public void Edit()
        {
            using (var db = new MyAnythingEntities())
            {
                var model = db.SystemField.Where(o => o.ItemCode == "BS").ToList();
                var TotalFeeModel = model.Where(o => o.ItemType == "TotalFee").FirstOrDefault();
                TotalFeeModel.ItemValue = TotalFee;
                var BuyFeeModel = model.Where(o => o.ItemType == "BuyFee").FirstOrDefault();
                BuyFeeModel.ItemValue = BuyFee;
                var UpperFeeModel = model.Where(o => o.ItemType == "UpperFee").FirstOrDefault();
                UpperFeeModel.ItemValue = UpperFee;
                var MinLimitAmtModel = model.Where(o => o.ItemType == "MinLimitAmt").FirstOrDefault();
                MinLimitAmtModel.ItemValue = MinLimitAmt;
                var HotelPromoFeeModel = model.Where(o => o.ItemType == "HotelPromoFee").FirstOrDefault();
                HotelPromoFeeModel.ItemValue = HotelPromoFee;
               
                db.SaveChanges();
            }
        }
    }
    
}