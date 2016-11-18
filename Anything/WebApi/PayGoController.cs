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
namespace Anything.WebApi
{
    public class PayGoController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("Order/PayNotify")]      
        public object PayNotify(PayGoRespond model)
        {
            using (var db = new MyAnythingEntities())
            {
                db.TEST.Add(new TEST { Message = model.Message, Created = DateTime.Now });
                db.SaveChanges();
            }
            using (var db = new MyAnythingEntities())
            {
                db.TEST.Add(new TEST { Message = model.Result, Created = DateTime.Now });
                db.SaveChanges();
            }
            var result = JsonConvert.DeserializeObject<PayResult>(model.Result);
            try
            {

                using (var db = new MyAnythingEntities())
                {


                    var Order = db.OrderMaster.Where(o => o.MerchantOrderNo == result.MerchantOrderNo).FirstOrDefault();
                    if (Order != null)
                    {
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
            return Json(new { Message="SUCCESS",Status="SUCCESS" });
        }
    }
}
