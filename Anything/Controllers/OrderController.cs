using Anything.Helper;
using Anything.Models;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int id, string key)
        {
            #region **檢查**
            if (Session[key] == null)
            {
                return RedirectToAction("Detail", "Home", new { id = id });
            }
            var AmountSelect = (List<SelectedAmount>)Session[key];
           
            var RoomItem = AmountSelect.Where(o => o.RoomId == id).FirstOrDefault();
            if (RoomItem == null)
            {
                return RedirectToAction("Detail", "Home", new { id = id });
            }

            var Now = DateTime.Now;
            DateTime checkInDate = Now;
            DateTime checkOutDate = Now.AddDays(1);

            if (Session["CheckInDate"] != null)
            {
                checkInDate = (DateTime)Session["CheckInDate"];
                checkOutDate = (DateTime)Session["CheckOutDate"];
            }

            #endregion

            var orderAmt = _db.OrderMaster.Where(o => o.ProductType == "Room" && o.ProductId == id && (o.CheckIn <= checkInDate && o.CheckOut > checkInDate)).Count();

            

            var amount = RoomItem.Amount <= 0 ? 0 : RoomItem.Amount;
           
            var RoomQuery = _db.Room.Find(id);

            if (orderAmt > RoomQuery.Quantity)
            {
                return RedirectToAction("Detail", "Home", new { id = id });
            }

            var result = new OrderViewModel();
            result.HotelName = RoomQuery.Hotel.Name;
            result.RoomId = RoomQuery.ID;
            result.RoomName = RoomQuery.Name;
            result.Price = RoomQuery.FixedPrice;
            result.Amount = amount;

            result.Total = result.Price * amount;
            
            //注意日期
            if (Session["CheckInDate"] != null)
            {
                result.CheckIn = (DateTime)Session["CheckInDate"];
                result.CheckOut = (DateTime)Session["CheckOutDate"];              
            }
            else
            {
                result.CheckIn = DateTime.Now;
                result.CheckOut = DateTime.Now.AddDays(1);
            }
               
            
            result.Key = key;
            return View(result);
        }

        [HttpPost]
        public ActionResult Create(OrderViewModel model)
        {

        #region **檢查**
            if (model.Key == null || string.IsNullOrEmpty(model.Key))
            {
                return RedirectToAction("Detail", "Home", new { id = model.RoomId });
            }

            var AmountSelect = (List<SelectedAmount>)Session[model.Key];

            var RoomItem = AmountSelect.Where(o => o.RoomId == model.RoomId).FirstOrDefault();
            if (RoomItem == null)
            {
                return RedirectToAction("Detail", "Home", new { id = model.RoomId });
            }

        #endregion


            var amount = RoomItem.Amount <= 0 ? 0 : RoomItem.Amount;
            var Now = DateTime.Now;
            var SysParameters = _db.SysManage.ToList();

            var bussinessBonus = SysParameters.Where(o=>o.FieldCode=="BoughtBonus").FirstOrDefault();
            decimal bussinessBonusValue = 0.0M;
            if(bussinessBonus!=null){
                bussinessBonusValue = decimal.Parse((double.Parse(bussinessBonus.Value)/100).ToString());
            }

             var SystemFee = SysParameters.Where(o=>o.FieldCode=="SystemFee").FirstOrDefault();
             decimal SystemFeeValue = 0.0M;
            if(SystemFee!=null){
                SystemFeeValue = decimal.Parse((double.Parse(SystemFee.Value) / 100).ToString());
            }

            var ShareBonus = SysParameters.Where(o => o.FieldCode == "UplineBonus").FirstOrDefault();
            decimal ShareBonusValue = 0.0M;
            if (ShareBonus != null)
            {
                ShareBonusValue = decimal.Parse((double.Parse(ShareBonus.Value) / 100).ToString());
            }

            var result = _db.Room.Find(model.RoomId);
            var Order = new OrderMaster();
            Order.Address = model.Address;
            Order.ProductName = result.Name;
            Order.BoughtBonus = bussinessBonusValue;
            Order.SystemBonus = SystemFeeValue;
            Order.ShareBonus = ShareBonusValue;
            Order.Amount = amount;

            var MerchantTradeNo = "R" + Guid.NewGuid().GetHashCode().ToString("x").ToLower();

            Order.MerchantTradeNo = MerchantTradeNo;
            Order.CheckIn = model.CheckIn;
            Order.CheckOut = model.CheckOut;
            
            Order.ProductId = model.RoomId;
            
            Order.Total = Order.Amount * model.Price;
            Order.UserId = CurrentUser.Id;
            Order.Tel = model.Tel;
            Order.Status = "1";
            Order.Remark = model.Remark;
            Order.Phone = model.Phone;
            //天數
            TimeSpan Total = Order.CheckOut.Subtract(Order.CheckIn);
            var d = Total.Days.ToString();
            Order.Modify = CurrentUser.Id;
            Order.Modified = Now;
            Order.Created = Now;
            //消費紅利
            Order.BoughtBonus = (int)(Order.Total * bussinessBonusValue);

            Order.PayVendor = "allpay";
            Order.Price = model.Price;
            Order.ProductType = "Room";

            if (ModelState.IsValid)
            {
                _db.OrderMaster.Add(Order);
                _db.SaveChanges();
                //return RedirectToAction("轉道付款");

                var pay = new AllPayModel();
                pay.MerchantTradeNo = MerchantTradeNo;
                pay.TotalAmount = Order.Total;
                pay.InstallmentAmount = 6;
                var ClientBackURL = string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority,"MyOrders");
                pay.ClientBackURL = ClientBackURL;
                var Room = _db.Room.Find(Order.ProductId);
                var Desc = string.Format("{0}/{1}/{2}/{3}", Room.Hotel.ID, Room.Hotel.Name, Room.ID, Room.Name);
                pay.TradeDesc = Desc;
                PayItem item = new PayItem();
                
                pay.Items.Add(new PayItem {Name = Order.ProductName.ToString(), Currency ="NT", Price = Order.Total, Quantity = Order.Amount });

                var HtmlCode = new _AllPay().PayOfOrder(pay);

                return Content(HtmlCode);
            }
            return View();
        }

        [HttpPost]
        public JsonResult SetSelectedAmount(SelectedAmount model)
        {
            #region **檢查**
            if (Session[model.key] == null)
            {
                RedirectToAction("Detail", "Home", new { id = model.RoomId });
            }

            var result = (List<SelectedAmount>)Session[model.key];

            var db_model = _db.Room.Where(o => o.ID == model.RoomId).FirstOrDefault();
            if (db_model == null)
            {
                RedirectToAction("Detail", "Home", new { id = model.RoomId });
            }

            var Order_Amount = _db.OrderMaster.GroupBy(o => o.ProductId == model.RoomId).Select(g => new { id = g.Key, total = g.Sum(i => i.Amount) }).FirstOrDefault().total;
            
            if (Order_Amount >= model.Amount)
            {
                RedirectToAction("Detail", "Home", new { id = model.RoomId });
            }
            #endregion

            foreach (var item in result)
            {
                if (item.RoomId == model.RoomId)
                {
                    item.Amount = model.Amount;
                    break;
                }
            }

            Session[model.key] = result;
            return Json(new {success=true,message="success" });
        }

        
    }
}