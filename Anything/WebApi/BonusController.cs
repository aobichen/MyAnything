using Anything.Helpers;
using Anything.Models;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Anything.WebApi
{
    public class BonusController : ApiController
    {
        [HttpPost]
        [Route("Bonus/Update")]
        public void UpdateBonus()
        {
            var Today = DateTime.Now;
            var CanUserStatus = BonusStatusEnum.CanUse.ToString();
            var db = new MyAnythingEntities();
            var CanUserModel = db.MyBonus.Where(o => (o.UseMonth.Year == Today.Year && o.UseMonth.Month == Today.Month) && o.BonusStatus != CanUserStatus).ToList();
            if (CanUserModel != null && CanUserModel.Count > 0)
            {
                foreach (var item in CanUserModel)
                {
                    item.BonusStatus = CanUserStatus;
                }
                db.SaveChanges();
            }

            var unPaid = OrderType.Unpaid.ToString();
            var ExpiredOrder = db.OrderMaster.Where(o => o.Status == unPaid &&
                    (o.ExpireDate ==null || DateTime.Compare(o.ExpireDate.Value, Today) < 0 )).ToList();
            if (ExpiredOrder != null && ExpiredOrder.Count > 0)
            {
                foreach (var item in ExpiredOrder)
                {
                    item.Status = OrderType.Expired.ToString();
                }
                db.SaveChanges();
            }
            
            db.Dispose();
        }

        [HttpPost]
        [Route("Order/ExpireDate")]
        public void ExpireDate()
        {
            var Today = DateTime.Now;
            var db = new MyAnythingEntities();
            var unPaid = OrderType.Unpaid.ToString();
            var ExpiredOrder = db.OrderMaster.Where(o => o.Status == unPaid &&
                    (o.ExpireDate == null || DateTime.Compare(o.ExpireDate.Value, Today) < 0)).ToList();
            if (ExpiredOrder != null && ExpiredOrder.Count > 0)
            {
                foreach (var item in ExpiredOrder)
                {
                    item.Status = OrderType.Expired.ToString();
                }
                db.SaveChanges();
            }

            db.Dispose();
        }
    }


}
