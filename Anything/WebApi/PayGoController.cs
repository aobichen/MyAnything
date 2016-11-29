using Anything.Models;
using Anything.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft;
using System.Web;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
namespace Anything.WebApi
{
    public class PayGoController : ApiController
    {
       
        [HttpPost]
        [Route("Order/PayNotify")]
        public object PayNotify()
        {
            var str = HttpContext.Current.Request["JSONData"];
            var model = JsonConvert.DeserializeObject<PayGoRespond>(str);
            var result = JsonConvert.DeserializeObject<PayResult>(model.Result);
            string PaymentType = result.PaymentType;
            DateTime? PayTime = string.IsNullOrEmpty(result.PayTime) ? (DateTime?)null : Convert.ToDateTime(result.PayTime);
            try
            {

                using (var db = new MyAnythingEntities())
                {

                    var Order = db.OrderMaster.Where(o => o.MerchantOrderNo == result.MerchantOrderNo).FirstOrDefault();
                    if (Order != null)
                    {
                        var PayGo = db.PayGo.Where(o => o.MerchantOrderNo == Order.MerchantOrderNo).FirstOrDefault();
                       
                        if (PayGo == null)
                        {
                            #region ## 新增 ##
                            db.PayGo.Add(new PayGo
                            {
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
                                IP = result.IP,
                                MerchantID = result.MerchantID,
                                MerchantOrderNo = result.MerchantOrderNo,
                                Message = model.Message,
                                PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code,
                                PayBankCode = result.BankCode == null ? string.Empty : result.BankCode,
                                PayTime = DateTime.Parse(result.PayTime),
                                RedAmt = result.RedAmt,
                                RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode,
                                Status = model.Status == null ? string.Empty : model.Status,
                                RespondType = result.RespondType == null ? string.Empty : result.RespondType,
                                TokenUseStatus = result.TokenUseStatus,
                                TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo
                            });

                            db.SaveChanges();
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
                            //db.SaveChanges();
                            #endregion
                        }

                        //var PayTime = string.IsNullOrEmpty(result.PayTime) ? DateTime.MinValue : DateTime.Parse(result.PayTime);
                       
                        if (model.Status.Equals("SUCCESS") &&
                            !string.IsNullOrEmpty(result.PayTime) &&
                            PayTime > DateTime.MinValue
                            && !db.MyBouns.Any(o=>o.MerchantOrderNo == Order.MerchantOrderNo))
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

                    
                }
            }
            catch (Exception ex)
            {
                using (var db = new MyAnythingEntities())
                {
                    db.TEST.Add(new TEST { Message = ex.Message.ToString(), Created = DateTime.Now });
                    db.SaveChanges();
                }
            }
            return Json(new { Message="SUCCESS",Status=true });
        }

        //[HttpPost]
        //[Route("Order/PaySuccess")]
        //public object ReturnURL()
        //{
        //    var str = HttpContext.Current.Request["JSONData"];
        //    var model = JsonConvert.DeserializeObject<PayGoRespond>(str);
        //    var result = JsonConvert.DeserializeObject<PayResult>(model.Result);

        //    try
        //    {

        //        using (var db = new MyAnythingEntities())
        //        {

        //            var Order = db.OrderMaster.Where(o => o.MerchantOrderNo == result.MerchantOrderNo).FirstOrDefault();
        //            if (Order != null)
        //            {
        //                var PayGo = db.PayGo.Where(o => o.MerchantOrderNo == Order.MerchantOrderNo).FirstOrDefault();

        //                if (PayGo == null)
        //                {
        //                    #region ## 新增 ##
        //                    db.PayGo.Add(new PayGo
        //                    {
        //                        Amt = result.Amt,
        //                        Auth = result.Auth == null ? string.Empty : result.Auth,
        //                        Barcode_1 = result.Barcode1 == null ? string.Empty : result.Barcode1,
        //                        Barcode_2 = result.Barcode2 == null ? string.Empty : result.Barcode2,
        //                        Barcode_3 = result.Barcode3 == null ? string.Empty : result.Barcode3,
        //                        Card4No = result.Card4No == null ? string.Empty : result.Card4No,
        //                        Card6No = result.Card6No == null ? string.Empty : result.Card6No,
        //                        CodeNo = result.CodeNo == null ? string.Empty : result.CodeNo,
        //                        EscrowBank = result.EscrowBank == null ? string.Empty : result.EscrowBank,
        //                        Inst = result.Inst,
        //                        InstEach = result.InstEach,
        //                        InstFirst = result.InstFirst,
        //                        IP = result.IP,
        //                        MerchantID = result.MerchantID,
        //                        MerchantOrderNo = result.MerchantOrderNo,
        //                        Message = model.Message,
        //                        PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code,
        //                        PayBankCode = result.BankCode == null ? string.Empty : result.BankCode,
        //                        PayTime = DateTime.Parse(result.PayTime),
        //                        RedAmt = result.RedAmt,
        //                        RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode,
        //                        Status = model.Status == null ? string.Empty : model.Status,
        //                        RespondType = result.RespondType == null ? string.Empty : result.RespondType,
        //                        TokenUseStatus = result.TokenUseStatus,
        //                        TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo
        //                    });

        //                    db.SaveChanges();
        //                    #endregion
        //                }
        //                else
        //                {
        //                    #region ## 更新 ##
        //                    PayGo.Amt = result.Amt;
        //                    PayGo.Auth = result.Auth == null ? string.Empty : result.Auth;
        //                    PayGo.Barcode_1 = result.Barcode1 == null ? string.Empty : result.Barcode1;
        //                    PayGo.Barcode_2 = result.Barcode2 == null ? string.Empty : result.Barcode2;
        //                    PayGo.Barcode_3 = result.Barcode3 == null ? string.Empty : result.Barcode3;
        //                    PayGo.Card4No = result.Card4No == null ? string.Empty : result.Card4No;
        //                    PayGo.Card6No = result.Card6No == null ? string.Empty : result.Card6No;
        //                    PayGo.CodeNo = result.CodeNo == null ? string.Empty : result.CodeNo;
        //                    PayGo.EscrowBank = result.EscrowBank == null ? string.Empty : result.EscrowBank;
        //                    PayGo.Inst = result.Inst;
        //                    PayGo.InstEach = result.InstEach;
        //                    PayGo.InstFirst = result.InstFirst;
        //                    PayGo.IP = result.IP;
        //                    PayGo.MerchantID = result.MerchantID;
        //                    PayGo.MerchantOrderNo = result.MerchantOrderNo;
        //                    PayGo.Message = model.Message;
        //                    PayGo.PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code;
        //                    PayGo.PayBankCode = result.BankCode == null ? string.Empty : result.BankCode;
        //                    PayGo.PayTime = DateTime.Parse(result.PayTime);
        //                    PayGo.RedAmt = result.RedAmt;
        //                    PayGo.RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode;
        //                    PayGo.Status = model.Status == null ? string.Empty : model.Status;
        //                    PayGo.RespondType = result.RespondType == null ? string.Empty : result.RespondType;
        //                    PayGo.TokenUseStatus = result.TokenUseStatus;
        //                    PayGo.TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo;
        //                    db.SaveChanges();
        //                    #endregion
        //                }

        //                var PayTime = string.IsNullOrEmpty(result.PayTime) ? DateTime.MinValue : DateTime.Parse(result.PayTime);

        //                if (model.Status.Equals("SUCCESS") &&
        //                    !string.IsNullOrEmpty(result.PayTime) &&
        //                    PayTime > DateTime.MinValue
        //                    && !db.MyBouns.Any(o => o.MerchantOrderNo == Order.MerchantOrderNo))
        //                {
        //                    var Bouns = new BounsViewModel();
        //                    Bouns.MerchantOrderNo = Order.MerchantOrderNo;
        //                    Bouns.OrderID = Order.ID;
        //                    Bouns.PayTime = PayTime;
        //                    Bouns.Status = model.Status;
        //                    Bouns.OrderAmt = Order.Amount;
        //                    Bouns.UseMonth = DateTime.Now.Month + 1;
        //                    Bouns.Create();
        //                }
        //            }


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        using (var db = new MyAnythingEntities())
        //        {
        //            db.TEST.Add(new TEST { Message = ex.Message.ToString(), Created = DateTime.Now });
        //            db.SaveChanges();
        //        }
        //    }
        //    return Json(new { Message = "SUCCESS", Status = true });
        //}

        //[HttpPost]
        //[Route("Order/PayCustomer")]
        //public object CutomerURL()
        //{
        //    var str = HttpContext.Current.Request["JSONData"];
        //    var model = JsonConvert.DeserializeObject<PayGoRespond>(str);
        //    var result = JsonConvert.DeserializeObject<PayResult>(model.Result);

        //    try
        //    {

        //        using (var db = new MyAnythingEntities())
        //        {

        //            var Order = db.OrderMaster.Where(o => o.MerchantOrderNo == result.MerchantOrderNo).FirstOrDefault();
        //            if (Order != null)
        //            {
        //                var PayGo = db.PayGo.Where(o => o.MerchantOrderNo == Order.MerchantOrderNo).FirstOrDefault();

        //                if (PayGo == null)
        //                {
        //                    #region ## 新增 ##
        //                    db.PayGo.Add(new PayGo
        //                    {
        //                        Amt = result.Amt,
        //                        Auth = result.Auth == null ? string.Empty : result.Auth,
        //                        Barcode_1 = result.Barcode1 == null ? string.Empty : result.Barcode1,
        //                        Barcode_2 = result.Barcode2 == null ? string.Empty : result.Barcode2,
        //                        Barcode_3 = result.Barcode3 == null ? string.Empty : result.Barcode3,
        //                        Card4No = result.Card4No == null ? string.Empty : result.Card4No,
        //                        Card6No = result.Card6No == null ? string.Empty : result.Card6No,
        //                        CodeNo = result.CodeNo == null ? string.Empty : result.CodeNo,
        //                        EscrowBank = result.EscrowBank == null ? string.Empty : result.EscrowBank,
        //                        Inst = result.Inst,
        //                        InstEach = result.InstEach,
        //                        InstFirst = result.InstFirst,
        //                        IP = result.IP,
        //                        MerchantID = result.MerchantID,
        //                        MerchantOrderNo = result.MerchantOrderNo,
        //                        Message = model.Message,
        //                        PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code,
        //                        PayBankCode = result.BankCode == null ? string.Empty : result.BankCode,
        //                        PayTime = DateTime.Parse(result.PayTime),
        //                        RedAmt = result.RedAmt,
        //                        RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode,
        //                        Status = model.Status == null ? string.Empty : model.Status,
        //                        RespondType = result.RespondType == null ? string.Empty : result.RespondType,
        //                        TokenUseStatus = result.TokenUseStatus,
        //                        TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo
        //                    });

        //                    db.SaveChanges();
        //                    #endregion
        //                }
        //                else
        //                {
        //                    #region ## 更新 ##
        //                    PayGo.Amt = result.Amt;
        //                    PayGo.Auth = result.Auth == null ? string.Empty : result.Auth;
        //                    PayGo.Barcode_1 = result.Barcode1 == null ? string.Empty : result.Barcode1;
        //                    PayGo.Barcode_2 = result.Barcode2 == null ? string.Empty : result.Barcode2;
        //                    PayGo.Barcode_3 = result.Barcode3 == null ? string.Empty : result.Barcode3;
        //                    PayGo.Card4No = result.Card4No == null ? string.Empty : result.Card4No;
        //                    PayGo.Card6No = result.Card6No == null ? string.Empty : result.Card6No;
        //                    PayGo.CodeNo = result.CodeNo == null ? string.Empty : result.CodeNo;
        //                    PayGo.EscrowBank = result.EscrowBank == null ? string.Empty : result.EscrowBank;
        //                    PayGo.Inst = result.Inst;
        //                    PayGo.InstEach = result.InstEach;
        //                    PayGo.InstFirst = result.InstFirst;
        //                    PayGo.IP = result.IP;
        //                    PayGo.MerchantID = result.MerchantID;
        //                    PayGo.MerchantOrderNo = result.MerchantOrderNo;
        //                    PayGo.Message = model.Message;
        //                    PayGo.PayAccount5Code = result.PayerAccount5Code == null ? string.Empty : result.PayerAccount5Code;
        //                    PayGo.PayBankCode = result.BankCode == null ? string.Empty : result.BankCode;
        //                    PayGo.PayTime = DateTime.Parse(result.PayTime);
        //                    PayGo.RedAmt = result.RedAmt;
        //                    PayGo.RespondCode = result.RespondCode == null ? string.Empty : result.RespondCode;
        //                    PayGo.Status = model.Status == null ? string.Empty : model.Status;
        //                    PayGo.RespondType = result.RespondType == null ? string.Empty : result.RespondType;
        //                    PayGo.TokenUseStatus = result.TokenUseStatus;
        //                    PayGo.TradeNo = result.TradeNo == null ? string.Empty : result.TradeNo;
        //                    db.SaveChanges();
        //                    #endregion
        //                }

        //                var PayTime = string.IsNullOrEmpty(result.PayTime) ? DateTime.MinValue : DateTime.Parse(result.PayTime);

        //                if (model.Status.Equals("SUCCESS") &&
        //                    !string.IsNullOrEmpty(result.PayTime) &&
        //                    PayTime > DateTime.MinValue
        //                    && !db.MyBouns.Any(o => o.MerchantOrderNo == Order.MerchantOrderNo))
        //                {
        //                    var Bouns = new BounsViewModel();
        //                    Bouns.MerchantOrderNo = Order.MerchantOrderNo;
        //                    Bouns.OrderID = Order.ID;
        //                    Bouns.PayTime = PayTime;
        //                    Bouns.Status = model.Status;
        //                    Bouns.OrderAmt = Order.Amount;
        //                    Bouns.UseMonth = DateTime.Now.Month + 1;
        //                    Bouns.Create();
        //                }
        //            }


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        using (var db = new MyAnythingEntities())
        //        {
        //            db.TEST.Add(new TEST { Message = ex.Message.ToString(), Created = DateTime.Now });
        //            db.SaveChanges();
        //        }
        //    }
        //    return Json(new { Message = "SUCCESS", Status = true });
        //}
       
    }
}
