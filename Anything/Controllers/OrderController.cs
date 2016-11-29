using Anything.Models;
using Anything.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Anything.Controllers
{
    public class OrderController : BaseController
    {
      
        [AllowAnonymous]
        public ActionResult PayCustomer()
        {
            var str = Request["JSONData"];

            var model = JsonConvert.DeserializeObject<PayGoRespond>(str);
            var result = JsonConvert.DeserializeObject<PayResult>(model.Result);
            
            string PaymentType = result.PaymentType;
            DateTime? PayTime = string.IsNullOrEmpty(result.PayTime) ? (DateTime?)null : Convert.ToDateTime(result.PayTime);
            var Order = _db.OrderMaster.Where(o => o.MerchantOrderNo == result.MerchantOrderNo).FirstOrDefault();
            if (Order != null)
            {
                var PayGo = _db.PayGo.Where(o => o.MerchantOrderNo == Order.MerchantOrderNo).FirstOrDefault();

                if (PayGo == null)
                {
                    
                    #region ## 新增 ##

                    var p = new PayGo
                    {
                        PaymentType = PaymentType,
                        Amt = result.Amt,
                        Auth = result.Auth == null ? string.Empty : result.Auth,
                        Barcode_1 = result.Barcode1 == null ? string.Empty : result.Barcode1,
                        Barcode_2 = result.Barcode2 == null ? string.Empty : result.Barcode2,
                        Barcode_3 = result.Barcode3 == null ? string.Empty : result.Barcode3,
                        Card4No = result.Card4No == null ? string.Empty : result.Card4No,
                        Card6No = result.Card6No == null ? string.Empty : result.Card6No,
                        CodeNo = result.CodeNo == null ? string.Empty : result.CodeNo,
                        EscrowBank = result.EscrowBank == null ? string.Empty : result.EscrowBank,
                        Inst = result.Inst,
                        InstEach = result.InstEach,
                        InstFirst = result.InstFirst,
                        IP = string.IsNullOrEmpty(result.IP) ? "192.168.0.1" : result.IP,
                        MerchantID = result.MerchantID,
                        MerchantOrderNo = result.MerchantOrderNo,
                        Message = model.Message,
                        PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code,
                        PayBankCode = result.BankCode == null ? string.Empty : result.BankCode,
                        PayTime = PayTime,
                        RedAmt = result.RedAmt,
                        RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode,
                        Status = model.Status == null ? string.Empty : model.Status,
                        RespondType = result.RespondType == null ? string.Empty : result.RespondType,
                        TokenUseStatus = result.TokenUseStatus,
                        TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo

                    };

                    _db.PayGo.Add(p);

                    _db.SaveChanges();
                    #endregion
                }
                else
                {
                 
                    #region ## 更新 ##
                    PayGo.Amt = result.Amt;
                    PayGo.Auth = result.Auth == null ? string.Empty : result.Auth;
                    PayGo.Barcode_1 = result.Barcode1 == null ? string.Empty : result.Barcode1;
                    PayGo.Barcode_2 = result.Barcode2 == null ? string.Empty : result.Barcode2;
                    PayGo.Barcode_3 = result.Barcode3 == null ? string.Empty : result.Barcode3;
                    PayGo.Card4No = result.Card4No == null ? string.Empty : result.Card4No;
                    PayGo.Card6No = result.Card6No == null ? string.Empty : result.Card6No;
                    PayGo.CodeNo = result.CodeNo == null ? string.Empty : result.CodeNo;
                    PayGo.EscrowBank = result.EscrowBank == null ? string.Empty : result.EscrowBank;
                    PayGo.Inst = result.Inst;
                    PayGo.InstEach = result.InstEach;
                    PayGo.InstFirst = result.InstFirst;
                    PayGo.IP = result.IP;
                    PayGo.MerchantID = result.MerchantID;
                    PayGo.MerchantOrderNo = result.MerchantOrderNo;
                    PayGo.Message = model.Message;
                    PayGo.PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code;
                    PayGo.PayBankCode = result.BankCode == null ? string.Empty : result.BankCode;
                    PayGo.PayTime = PayTime;
                    PayGo.RedAmt = result.RedAmt;
                    PayGo.RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode;
                    PayGo.Status = model.Status == null ? string.Empty : model.Status;
                    PayGo.RespondType = result.RespondType == null ? string.Empty : result.RespondType;
                    PayGo.TokenUseStatus = result.TokenUseStatus;
                    PayGo.TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo;
                    PayGo.PaymentType = PaymentType;
                    PayGo.IP = string.IsNullOrEmpty(result.IP) ? "192.168.0.1" : result.IP;
                    
                    _db.SaveChanges();
                    #endregion
                }

                if (model.Status.Equals("SUCCESS") &&
                           !string.IsNullOrEmpty(result.PayTime) &&
                           PayTime > DateTime.MinValue
                           && !_db.MyBouns.Any(o => o.MerchantOrderNo == Order.MerchantOrderNo))
                {
                    var Bouns = new BounsViewModel();
                    Bouns.MerchantOrderNo = Order.MerchantOrderNo;
                    Bouns.OrderID = Order.ID;
                    Bouns.PayTime = PayTime;
                    Bouns.Status = model.Status;
                    Bouns.OrderAmt = Order.Amount;
                    Bouns.UseMonth = DateTime.Now.Month + 1;
                    Bouns.UserID = Order.UserId;
                    Bouns.Create();
                }
            }

            var PayModel = (from order in _db.OrderMaster
                            //join h in _db.Hotel
                            // on order.ProductId equals RoomId
                            join pay in _db.PayGo
                            on order.MerchantOrderNo equals pay.MerchantOrderNo
                            select new OrderViewModel
                            {
                                Address = order.Address,
                                Amount = order.Amount,
                                CheckIn = order.CheckIn,
                                CheckOut = order.CheckOut,
                                //HotelName = h.Name,
                                MerchantTradeNo = order.MerchantOrderNo,
                                Phone = order.Tel,
                                RoomId = order.ProductId,
                                RoomName = order.ProductName,
                                PayGo = pay,
                                UserId = order.UserId,
                                Email = order.Email,
                                Quantity = order.Quantity,
                                Remark = order.Remark,
                                Name = order.Name,
                                PaymentType = pay.PaymentType,
                                CodeNo = pay.CodeNo,
                                Barcode_1 = pay.Barcode_1,
                                Barcode_2 = pay.Barcode_2,
                                Barcode_3 = pay.Barcode_3,
                                PayerAccount5Code = pay.PayAccount5Code,
                                PayBankCode = pay.PayBankCode

                            }).FirstOrDefault();


            PayModel.HotelName = _db.Room.Find(PayModel.RoomId).Hotel.Name;
            //PayModel.Name = Account_db.Users.Where(o => o.Id == PayModel.UserId).FirstOrDefault().UserName;
            return View(PayModel);
        }

        [AllowAnonymous]
        public ActionResult PaySuccess()
        {
            var str = Request["JSONData"];
            _db.TEST.Add(new TEST { Message = str,Created = DateTime.Now });
            _db.SaveChanges();
            var model = JsonConvert.DeserializeObject<PayGoRespond>(str);
            var result = JsonConvert.DeserializeObject<PayResult>(model.Result);          
            string PaymentType = result.PaymentType;
            DateTime? PayTime = string.IsNullOrEmpty(result.PayTime) ? (DateTime?)null : Convert.ToDateTime(result.PayTime);
            var Order = _db.OrderMaster.Where(o => o.MerchantOrderNo == result.MerchantOrderNo).FirstOrDefault();
            if (Order != null)
            {
                var PayGo = _db.PayGo.Where(o => o.MerchantOrderNo == Order.MerchantOrderNo).FirstOrDefault();

                if (PayGo == null)
                {
                   
                    #region ## 新增 ##

                    var p = new PayGo
                    {
                        PaymentType = PaymentType,
                        Amt = result.Amt,
                        Auth = result.Auth == null ? string.Empty : result.Auth,
                        Barcode_1 = result.Barcode1 == null ? string.Empty : result.Barcode1,
                        Barcode_2 = result.Barcode2 == null ? string.Empty : result.Barcode2,
                        Barcode_3 = result.Barcode3 == null ? string.Empty : result.Barcode3,
                        Card4No = result.Card4No == null ? string.Empty : result.Card4No,
                        Card6No = result.Card6No == null ? string.Empty : result.Card6No,
                        CodeNo = result.CodeNo == null ? string.Empty : result.CodeNo,
                        EscrowBank = result.EscrowBank == null ? string.Empty : result.EscrowBank,
                        Inst = result.Inst,
                        InstEach = result.InstEach,
                        InstFirst = result.InstFirst,
                       IP = string.IsNullOrEmpty(result.IP) ? "192.168.0.1" : result.IP,
                        MerchantID = result.MerchantID,
                        MerchantOrderNo = result.MerchantOrderNo,
                        Message = model.Message,
                        PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code,
                        PayBankCode = result.BankCode == null ? string.Empty : result.BankCode,
                        PayTime = PayTime,
                        RedAmt = result.RedAmt,
                        RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode,
                        Status = model.Status == null ? string.Empty : model.Status,
                        RespondType = result.RespondType == null ? string.Empty : result.RespondType,
                        TokenUseStatus = result.TokenUseStatus,
                        TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo

                    };

                    _db.PayGo.Add(p);
                   
                    _db.SaveChanges();
                    #endregion
                }
                else
                {
                   
                    #region ## 更新 ##
                    PayGo.Amt = result.Amt;
                    PayGo.Auth = result.Auth == null ? string.Empty : result.Auth;
                    PayGo.Barcode_1 = result.Barcode1 == null ? string.Empty : result.Barcode1;
                    PayGo.Barcode_2 = result.Barcode2 == null ? string.Empty : result.Barcode2;
                    PayGo.Barcode_3 = result.Barcode3 == null ? string.Empty : result.Barcode3;
                    PayGo.Card4No = result.Card4No == null ? string.Empty : result.Card4No;
                    PayGo.Card6No = result.Card6No == null ? string.Empty : result.Card6No;
                    PayGo.CodeNo = result.CodeNo == null ? string.Empty : result.CodeNo;
                    PayGo.EscrowBank = result.EscrowBank == null ? string.Empty : result.EscrowBank;
                    PayGo.Inst = result.Inst;
                    PayGo.InstEach = result.InstEach;
                    PayGo.InstFirst = result.InstFirst;
                    PayGo.IP = result.IP;
                    PayGo.MerchantID = result.MerchantID;
                    PayGo.MerchantOrderNo = result.MerchantOrderNo;
                    PayGo.Message = model.Message;
                    PayGo.PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code;
                    PayGo.PayBankCode = result.BankCode == null ? string.Empty : result.BankCode;
                    PayGo.PayTime = PayTime;
                    PayGo.RedAmt = result.RedAmt;
                    PayGo.RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode;
                    PayGo.Status = model.Status == null ? string.Empty : model.Status;
                    PayGo.RespondType = result.RespondType == null ? string.Empty : result.RespondType;
                    PayGo.TokenUseStatus = result.TokenUseStatus;
                    PayGo.TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo;
                    PayGo.PaymentType = PaymentType;
                    PayGo.IP = string.IsNullOrEmpty(result.IP) ? "192.168.0.1" : result.IP;
                    //_db.TEST.Add(new TEST { Message = Newtonsoft.Json.JsonConvert.SerializeObject(PayGo), Created = DateTime.Now });
                    _db.SaveChanges();
                    #endregion
                }
                if (model.Status.Equals("SUCCESS") &&
                            !string.IsNullOrEmpty(result.PayTime) &&
                            PayTime > DateTime.MinValue
                            && !_db.MyBouns.Any(o => o.MerchantOrderNo == Order.MerchantOrderNo))
                {
                    var Bouns = new BounsViewModel();
                    Bouns.MerchantOrderNo = Order.MerchantOrderNo;
                    Bouns.OrderID = Order.ID;
                    Bouns.PayTime = PayTime;
                    Bouns.Status = model.Status;
                    Bouns.OrderAmt = Order.Amount;
                    Bouns.UseMonth = DateTime.Now.Month + 1;
                    Bouns.UserID = Order.UserId;
                    Bouns.Create();
                }
            }

            var PayModel = (from order in _db.OrderMaster
                            //join h in _db.Hotel
                            // on order.ProductId equals RoomId
                            join pay in _db.PayGo
                            on order.MerchantOrderNo equals pay.MerchantOrderNo
                            select new OrderViewModel
                            {
                                Address = order.Address,
                                Amount = order.Amount,
                                CheckIn = order.CheckIn,
                                CheckOut = order.CheckOut,
                                //HotelName = h.Name,
                                MerchantTradeNo = order.MerchantOrderNo,
                                Phone = order.Tel,
                                RoomId = order.ProductId,
                                RoomName = order.ProductName,
                                PayGo = pay,
                                UserId = order.UserId,
                                Email = order.Email,
                                Quantity = order.Quantity,
                                Remark = order.Remark,
                                Name = order.Name,
                                PaymentType = pay.PaymentType,
                                CodeNo = pay.CodeNo,
                                Barcode_1 = pay.Barcode_1,
                                Barcode_2 = pay.Barcode_2,
                                Barcode_3 = pay.Barcode_3,
                                PayerAccount5Code = pay.PayAccount5Code,
                                PayBankCode = pay.PayBankCode

                            }).FirstOrDefault();
            

            PayModel.HotelName = _db.Room.Find(PayModel.RoomId).Hotel.Name;
            //PayModel.Name = Account_db.Users.Where(o => o.Id == PayModel.UserId).FirstOrDefault().UserName;
            return View(PayModel);
        }

        [AllowAnonymous]
        public ActionResult PayNotify()
        {
            var str = Request["JSONData"];

            var model = JsonConvert.DeserializeObject<PayGoRespond>(str);
            var result = JsonConvert.DeserializeObject<PayResult>(model.Result);
            // _db.TEST.Add(new TEST { Message = result.PaymentType, Created = DateTime.Now });
            // _db.SaveChanges();
            string PaymentType = result.PaymentType;
            DateTime? PayTime = string.IsNullOrEmpty(result.PayTime) ? (DateTime?)null : Convert.ToDateTime(result.PayTime);
            var Order = _db.OrderMaster.Where(o => o.MerchantOrderNo == result.MerchantOrderNo).FirstOrDefault();
            if (Order != null)
            {
                var PayGo = _db.PayGo.Where(o => o.MerchantOrderNo == Order.MerchantOrderNo).FirstOrDefault();

                if (PayGo == null)
                {
                   
                    #region ## 新增 ##

                    var p = new PayGo
                    {
                        PaymentType = PaymentType,
                        Amt = result.Amt,
                        Auth = result.Auth == null ? string.Empty : result.Auth,
                        Barcode_1 = result.Barcode1 == null ? string.Empty : result.Barcode1,
                        Barcode_2 = result.Barcode2 == null ? string.Empty : result.Barcode2,
                        Barcode_3 = result.Barcode3 == null ? string.Empty : result.Barcode3,
                        Card4No = result.Card4No == null ? string.Empty : result.Card4No,
                        Card6No = result.Card6No == null ? string.Empty : result.Card6No,
                        CodeNo = result.CodeNo == null ? string.Empty : result.CodeNo,
                        EscrowBank = result.EscrowBank == null ? string.Empty : result.EscrowBank,
                        Inst = result.Inst,
                        InstEach = result.InstEach,
                        InstFirst = result.InstFirst,
                        IP = string.IsNullOrEmpty(result.IP) ? "192.168.0.1" : result.IP,
                        MerchantID = result.MerchantID,
                        MerchantOrderNo = result.MerchantOrderNo,
                        Message = model.Message,
                        PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code,
                        PayBankCode = result.BankCode == null ? string.Empty : result.BankCode,
                        PayTime = PayTime,
                        RedAmt = result.RedAmt,
                        RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode,
                        Status = model.Status == null ? string.Empty : model.Status,
                        RespondType = result.RespondType == null ? string.Empty : result.RespondType,
                        TokenUseStatus = result.TokenUseStatus,
                        TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo

                    };

                    _db.PayGo.Add(p);

                    _db.SaveChanges();
                    #endregion
                }
                else
                {
                    
                    #region ## 更新 ##
                    PayGo.Amt = result.Amt;
                    PayGo.Auth = result.Auth == null ? string.Empty : result.Auth;
                    PayGo.Barcode_1 = result.Barcode1 == null ? string.Empty : result.Barcode1;
                    PayGo.Barcode_2 = result.Barcode2 == null ? string.Empty : result.Barcode2;
                    PayGo.Barcode_3 = result.Barcode3 == null ? string.Empty : result.Barcode3;
                    PayGo.Card4No = result.Card4No == null ? string.Empty : result.Card4No;
                    PayGo.Card6No = result.Card6No == null ? string.Empty : result.Card6No;
                    PayGo.CodeNo = result.CodeNo == null ? string.Empty : result.CodeNo;
                    PayGo.EscrowBank = result.EscrowBank == null ? string.Empty : result.EscrowBank;
                    PayGo.Inst = result.Inst;
                    PayGo.InstEach = result.InstEach;
                    PayGo.InstFirst = result.InstFirst;
                    PayGo.IP = result.IP;
                    PayGo.MerchantID = result.MerchantID;
                    PayGo.MerchantOrderNo = result.MerchantOrderNo;
                    PayGo.Message = model.Message;
                    PayGo.PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code;
                    PayGo.PayBankCode = result.BankCode == null ? string.Empty : result.BankCode;
                    PayGo.PayTime = PayTime;
                    PayGo.RedAmt = result.RedAmt;
                    PayGo.RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode;
                    PayGo.Status = model.Status == null ? string.Empty : model.Status;
                    PayGo.RespondType = result.RespondType == null ? string.Empty : result.RespondType;
                    PayGo.TokenUseStatus = result.TokenUseStatus;
                    PayGo.TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo;
                    PayGo.PaymentType = PaymentType;
                    PayGo.IP = string.IsNullOrEmpty(result.IP) ? "192.168.0.1" : result.IP;
                    //_db.TEST.Add(new TEST { Message = Newtonsoft.Json.JsonConvert.SerializeObject(PayGo), Created = DateTime.Now });
                    _db.SaveChanges();
                    #endregion
                }

                if (model.Status.Equals("SUCCESS") &&
                            !string.IsNullOrEmpty(result.PayTime) &&
                            PayTime > DateTime.MinValue
                            && !_db.MyBouns.Any(o => o.MerchantOrderNo == Order.MerchantOrderNo))
                {
                    var Bouns = new BounsViewModel();
                    Bouns.MerchantOrderNo = Order.MerchantOrderNo;
                    Bouns.OrderID = Order.ID;
                    Bouns.PayTime = PayTime;
                    Bouns.Status = model.Status;
                    Bouns.OrderAmt = Order.Amount;
                    Bouns.UseMonth = DateTime.Now.Month + 1;
                    Bouns.UserID = Order.UserId;
                    Bouns.Create();
                }
            }

            var PayModel = (from order in _db.OrderMaster
                            //join h in _db.Hotel
                            // on order.ProductId equals RoomId
                            join pay in _db.PayGo
                            on order.MerchantOrderNo equals pay.MerchantOrderNo
                            select new OrderViewModel
                            {
                                Address = order.Address,
                                Amount = order.Amount,
                                CheckIn = order.CheckIn,
                                CheckOut = order.CheckOut,
                                //HotelName = h.Name,
                                MerchantTradeNo = order.MerchantOrderNo,
                                Phone = order.Tel,
                                RoomId = order.ProductId,
                                RoomName = order.ProductName,
                                PayGo = pay,
                                UserId = order.UserId,
                                Email = order.Email,
                                Quantity = order.Quantity,
                                Remark = order.Remark,
                                Name = order.Name,
                                PaymentType = pay.PaymentType,
                                CodeNo = pay.CodeNo,
                                Barcode_1 = pay.Barcode_1,
                                Barcode_2 = pay.Barcode_2,
                                Barcode_3 = pay.Barcode_3,
                                PayerAccount5Code = pay.PayAccount5Code,
                                PayBankCode = pay.PayBankCode

                            }).FirstOrDefault();


            PayModel.HotelName = _db.Room.Find(PayModel.RoomId).Hotel.Name;
            //PayModel.Name = Account_db.Users.Where(o => o.Id == PayModel.UserId).FirstOrDefault().UserName;
            return View(PayModel);
        }
    }
}