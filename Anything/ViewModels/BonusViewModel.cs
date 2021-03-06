﻿using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Anything.Helpers;


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
                var UsedBonus = db.Bonus.Where(o => o.MerchantOrderNo == MerchantOrderNo).FirstOrDefault();
                if (UsedBonus != null)
                {
                    UsedBonus.Status = "active";
                    db.SaveChanges();
                }

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

                var UpperUser = Bonus.Where(o => o.ItemType == "UpperFee").FirstOrDefault();
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

                //平台總比例裡的5%為發送的部分
                var TotalPercent = 0.5;
                //發送金額 = 訂單金額 * 總比例(13.5)*0.01
                var TotalAmt = ((double)OrderAmt * (Total * 0.01)) * TotalPercent;
                //db.TEST.Add(new TEST { Created = DateTime.Now, Message = string.Format("TotalAmt1:{0}", Total*0.01) });
                //db.TEST.Add(new TEST { Created = DateTime.Now, Message = string.Format("TotalAmt:{0}",TotalAmt) });
                //消費帳號紅利 = 發送金額 * 0.3
                var BuyAmt = Math.Floor(TotalAmt * (Buy * 0.01));
                //db.TEST.Add(new TEST { Created = DateTime.Now, Message = string.Format("BuyAmt1:{0}", Buy * 0.01) });
                //db.TEST.Add(new TEST { Created = DateTime.Now, Message = string.Format("BuyAmt:{0}", BuyAmt) });
                //民宿推薦帳號紅利 = 發送金額 * 0.05
                var HotelAmt = Math.Floor(TotalAmt * (Hotel * 0.01));
                //db.TEST.Add(new TEST { Created = DateTime.Now, Message = string.Format("HotelAmt1:{0}", Hotel * 0.01) });
                //db.TEST.Add(new TEST { Created = DateTime.Now, Message = string.Format("HotelAmt:{0}", HotelAmt) });
                //上線平均紅利 = (發送金額 * 0.3)/6
                var UpperAmt = Math.Floor((TotalAmt * (Upper * 0.01)) / 6);
                //db.TEST.Add(new TEST { Created = DateTime.Now, Message = string.Format("HotelAmt1:{0}", Upper * 0.01) });
                //db.TEST.Add(new TEST { Created = DateTime.Now, Message = string.Format("UpperAmt:{0}", UpperAmt) });

                var LimitAmt = MinAmt;
                

                var Today = DateTime.Now;

                var NextMonth = Today.AddMonths(1);
                db.TEST.Add(new TEST { Created = Today, Message = NextMonth.ToString() });
                db.SaveChanges();

                var HotelRecommandID = 0;
                //更新付款狀態為已付款
                var Order = db.OrderMaster.Where(o => o.MerchantOrderNo == MerchantOrderNo).FirstOrDefault();
                if (Order != null)
                {
                    Order.Status = OrderType.Paid.ToString();
                    var Room = db.Room.Find(Order.ProductId);
                    using (var _db = new ApplicationDbContext())
                    {
                        var user = _db.Users.Where(o=>o.Id == Room.Hotel.UserId).FirstOrDefault();
                        if (user != null && !string.IsNullOrEmpty(user.Recommend))
                        {
                            var Recommand = _db.Users.Where(o => o.UserCode == user.Recommend).FirstOrDefault();
                            if (Recommand != null)
                            {
                                HotelRecommandID = Recommand.Id;
                            }
                        }
                    }
                }
               
                db.MyBonus.Add(new MyBonus
                {
                    AmtMinLimit = (int)MinAmt,
                    Bonus = (decimal)BuyAmt,
                    Created = Today,
                    OrderAmt = OrderAmt.Value,
                    OrderID = OrderID.Value,
                    PayTime = PayTime.Value,
                    PayStatus = Status,
                    UseMonth = NextMonth,
                    UserID = UserID.Value,
                    MerchantOrderNo = MerchantOrderNo,
                    ParentID =0,
                    BonusType = BonusTypeEnum.Purchasing.ToString(),
                    BonusStatus = BonusStatusEnum.CanUse.ToString(),
                    Notified = false
                });

                if (HotelRecommandID > 0)
                {
                    db.MyBonus.Add(new MyBonus
                    {
                        AmtMinLimit = (int)MinAmt,
                        Bonus = (decimal)HotelAmt,
                        Created = Today,
                        OrderAmt = OrderAmt.Value,
                        OrderID = OrderID.Value,
                        PayTime = PayTime.Value,
                        PayStatus = Status,
                        UseMonth = NextMonth,
                        UserID = HotelRecommandID,
                        MerchantOrderNo = MerchantOrderNo,
                        ParentID = 0,
                        BonusType = BonusTypeEnum.HotelRecomend.ToString(),
                        BonusStatus = BonusStatusEnum.CurrentBonus.ToString(),
                        Notified = false
                    });
                }


                var UpperUsers = new UpperUserModel().GETUpperUserList(Order.UserId.Value);
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
                        UseMonth = NextMonth,
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
            //UpdateExpired();
        }

        /// <summary>
        /// 更新繳費期限過期的訂單
        /// </summary>
        public void UpdateExpired()
        {
            var unPaid = OrderType.Unpaid.ToString();
            using (var db = new MyAnythingEntities())
            {
                var Today = DateTime.Now;
                var ExpiredOrder = db.OrderMaster.Where(o => o.Status == unPaid && (o.ExpireDate.Value.Year >= Today.Year && o.ExpireDate.Value.Month >= Today.Month && o.ExpireDate.Value.Date >= Today.Date)).ToList();
                if (ExpiredOrder != null && ExpiredOrder.Count > 0)
                {
                    foreach (var item in ExpiredOrder)
                    {
                        item.Status = OrderType.Expired.ToString();
                    }
                    db.SaveChanges();
                }
            }
        }

        public void Nodified()
        {

        }
    }


    public enum BonusTypeEnum
    {
        None =0,
        Purchasing,
        Recommend,
        HotelRecomend
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