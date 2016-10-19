
using AllPay.Payment.Integration;
using Anything.Helper;
using Anything.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    public class AllPayController : BaseController
    {
        // GET: AllPay
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CreditFeed()
        {
            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;
            var feeds = new AllPayFeed();
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    oPayment.HashKey = "CAIgOXDxEY2CNpYD";
                    oPayment.HashIV = "deLsiLkdNntXFaHq";


                    /* 取回付款結果 */
                    enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
                }
                // 取回所有資料
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
                            default: break;
                        }
                    }

                    _db.AllPayFeed.Add(feeds);
                    _db.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                feeds.MerchantID = "A00001";
                _db.AllPayFeed.Add(feeds);
                _db.SaveChanges();
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                feeds.MerchantID = "A00002";
                _db.AllPayFeed.Add(feeds);
                _db.SaveChanges();
                this.Response.Clear();
                // 回覆成功訊息。
                if (enErrors.Count() == 0)
                    this.Response.Write("1|OK");
                // 回覆錯誤訊息。
                else
                    this.Response.Write(String.Format("0|{0}", String.Join("\\r\\n", enErrors)));
                this.Response.Flush();
                this.Response.End();
            }

            return View();
        }
        
}
    }
        
