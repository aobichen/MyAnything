using Anything.Models;
using Anything.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Anything.WebApi
{
    public class PayGoController : ApiController
    {
        [HttpPost]
        [Route("PayNotify")]      
        public object PayNotify(PayGoRespond model)
        {
            using (var db = new MyAnythingEntities())
            {
                var Order = db.OrderMaster.Where(o => o.MerchantOrderNo == model.MerchantOrderNo).FirstOrDefault();
                if (Order != null)
                {
                    db.PayGo.Add(new PayGo
                    {
                        Amt = model.Amt,
                        Auth = model.Auth,
                        Barcode_1 = model.Barcode1,
                        Barcode_2 = model.Barcode2,
                        Barcode_3 = model.Barcode3,
                        Card4No = model.Card4No,
                        Card6No = model.Card6No,
                        CodeNo = model.CoedNo,
                        EscrowBank = model.EscrowBank,
                        Inst = model.Inst,
                        InstEach = model.InstEach,
                        InstFirst = model.InstFirst,
                        IP = model.IP,
                        MerchantID = model.MerchantID,
                        MerchantOrderNo = model.MerchantOrderNo,
                        Message = model.Message,
                        PayAccount5Code = model.PayerAccount5Code,
                        PayBankCode = model.PayBankCode,
                        PayTime = model.PayTime,
                        RedAmt = model.RedAmt,
                        RespondCode = model.RespondCode,
                        Status = model.Status,
                        RespondType = model.RespondType,
                        TokenUseStatus = model.TokenUseStatus,
                        TradeNo = model.TradeNo
                    });

                    db.SaveChanges();
                }
            }
            return new string[] { "value1", "value2" };
        }
    }
}
