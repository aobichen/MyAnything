using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Helpers
{
    public enum OrderType
    {
        None = 0,
        Unpaid,
        Paid,
        Expired
    }

   

    public enum RoomPriceType
    {
        Day = 0,
        Holiday,
        Fixed       
    }

    public class ParseEnum{
      public static string ParseBonusStatus(string status){
            //var StatusEnum = BonusStatusEnum.
                var value = string.Empty;
                if (status.Equals(BonusStatusEnum.CanUse.ToString()))
                {
                    value = "可以使用";
                }
                else if (status.Equals(BonusStatusEnum.CurrentBonus.ToString()))
                {
                    value = "新紅利(無法使用)";
                }else if (status.Equals(BonusStatusEnum.NoOverAmount.ToString()))
                {
                    value = "未達當月消費金額(無法使用)";
                }
                return value;
              }
      public static string ParseBonusType(string status)
      {
          //var StatusEnum = BonusStatusEnum.
          var value = string.Empty;
          if (status.Equals(BonusTypeEnum.HotelRecomend.ToString()))
          {
              value = "推薦民宿紅利";
          }
          else if (status.Equals(BonusTypeEnum.Purchasing.ToString()))
          {
              value = "購物紅利";
          }
          else if (status.Equals(BonusTypeEnum.Recommend.ToString()))
          {
              value = "推薦紅利";
          }
          return value;
      }

      public static string ParsePayStatusType(string status)
      {
          //var StatusEnum = BonusStatusEnum.
          var value = string.Empty;
          status = status.ToLower();
          switch (status)
          {
              case "paid":
                  value = "已付款";
                  break;
              case "expired":
                  value = "付款期限已過";
                  break;
              case "unpaid":
                  value = "未付款";
                  break;
             
          }
          
          return value;
      }
   }
    
   
}