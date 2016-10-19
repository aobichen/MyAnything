using AllPay.Payment.Integration;
using Anything.Helper;
using Anything.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Anything.WebApi
{
    public class AllPayController : ApiController
    {
        private AnythingEntities _db { get; set; }
        public AllPayController()
        {
            _db = new AnythingEntities();
        }

        [Route("CreditFeed")]
        [HttpPost]
        public HttpResponseMessage CreditFeed()
        {
            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;
            var feeds = new AllPayFeed();
            var response = new HttpResponseMessage();
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {

                    oPayment.HashKey = "CAIgOXDxEY2CNpYD";
                    oPayment.HashIV = "deLsiLkdNntXFaHq";
                    enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
                }

                if (enErrors.Count() == 0)
                {

                    foreach (string szKey in htFeedback.Keys)
                    {
                        switch (szKey)
                        {
                            /* 支付後的回傳的基本參數 */
                            case "MerchantID": feeds.MerchantID = htFeedback[szKey].ToString(); break;
                            case "MerchantTradeNo": feeds.MerchantTradeNo = htFeedback[szKey].ToString();
                                break;
                            case "PaymentDate": feeds.PaymentDate = htFeedback[szKey].ToString(); break;
                            case "PaymentType": feeds.PaymentType = htFeedback[szKey].ToString(); break;
                            case "PaymentTypeChargeFee": feeds.PaymentTypeChargeFee =
                           htFeedback[szKey].ToString(); break;
                            case "RtnCode": feeds.RtnCode = htFeedback[szKey].ToString(); break;
                            case "RtnMsg": feeds.RtnMsg = htFeedback[szKey].ToString(); break;
                            case "SimulatePaid": feeds.SimulatePaid = htFeedback[szKey].ToString(); break;
                            case "TradeAmt": feeds.TradeAmt = htFeedback[szKey].ToString(); break;
                            case "TradeDate": feeds.TradeDate = htFeedback[szKey].ToString(); break;
                            case "TradeNo": feeds.TradeNo = htFeedback[szKey].ToString(); break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (feeds.MerchantID == "")
                {
                    feeds.MerchantID = "A0001";
                }
                _db.AllPayFeed.Add(feeds);
                _db.SaveChanges();

               System.Web.HttpContext.Current.Response.Clear();
               if (enErrors.Count() == 0)
               {
                   
                   
                   response.Content = new StringContent("1|OK");
                   response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                   
               }
               else
               {
                   
                   response.Content = new StringContent(string.Format("0|{0}", string.Join("\\r\\n", enErrors)));
                   response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
               }
            }

            return response;
        }

        [Route("OrderQuery")]
        [HttpPost]
        public void AllPayFeedBack()
        {
            var order = _db.OrderMaster.Where(o => o.TradeNo == null || o.TradeStatus=="0").ToList();
            var allpay = new _AllPay();
            foreach (var item in order)
            {
                allpay.PayOfFeedBack(item.MerchantTradeNo);
            }
        }
    }
}
