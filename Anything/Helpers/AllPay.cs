using AllPay.Payment.Integration;
using Anything.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Anything.Helpers
{
    public class AllPayFeedViewModel
    {
        //廠商編號
        public string MerchantID { get; set; }
        //廠商交易編號
        public string MerchantTradeNo { get; set; }
        //交易狀態
        public string RtnCode { get; set; }
        //交易訊息
        public string RtnMsg { get; set; }
        //會員付款方式
        public string PaymentType { get; set; }

        public string PaymentDate { get; set; }

        //通路費
        public string PaymentTypeChargeFee { get; set; }

        //模擬付款
        public string SimulatePaid { get; set; }
        //交易金額
        public string TradeAmt { get; set; }
        //訂單成立時間
        public string TradeDate { get; set; }
        //AllPay交易編號
        public string TradeNo { get; set; }
    }

    public class AllPayModel
    {
        public AllPayModel()
        {
            if (Items == null)
            {
                Items = new List<PayItem>();
            }
        }
        public string ReturnURL { get; set; }
        public string ClientBackURL { get; set; }

        //訂單交易編號
        public string MerchantTradeNo { get; set; }
        public string OrderResultURL { get; set; }
        public DateTime MerchantTradeDate { get { return DateTime.Now; } }

        public Decimal TotalAmount { get; set; }
        public string TradeDesc { get; set; }

        public PaymentMethod ChoosePayment { get; set; }
        public string Remark { get; set; }

        public PaymentMethodItem ChooseSubPayment { get; set; }
        public ExtraPaymentInfo NeedExtraPaidInfo { get; set; }
        public HoldTradeType HoldTrade { get; set; }
        public DeviceType DeviceSource { get; set; }

        public int CreditInstallment { get; set; }
        public decimal InstallmentAmount { get; set; }

        public string PaymentType { get; set; }
        public string TradeStatus { get; set; }
        //是否使用紅利折抵
        //oPayment.SendExtend.Redeem = Boolean.Parse("false");
        //是否使用銀聯卡
        //oPayment.SendExtend.UnionPay = Boolean.Parse("false");
        public List<PayItem> Items { get; set; }

        public Anything.Helpers._AllPay.PayMethod PayMethod { get; set; }
    }

    public class PayItem
    {
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public string Currency { get; set; }
        public int Quantity { get; set; }
        public string URL { get; set; }

    }

    public class _AllPay
    {
        public string HtmlCode { get; set; }

        //private string HashKey ="5294y06JbISpM5x9";
        //private string HashVI = "v77hoKGq4kWxNNIS";
        //private string MerchantID = "2000132";

        private const string HashKey = "CAIgOXDxEY2CNpYD";
        private const string HashIV = "deLsiLkdNntXFaHq";
        private const string MerchantID = "1446800";
        private const string ServiceURL = "https://payment.allpay.com.tw/Cashier/AioCheckOut";
        private const string ReturnURL = "http://www.anything.somee.com/CreditFeed";

        //oPayment.ServiceURL = "https://payment-stage.allpay.com.tw/Cashier/AioCheckOut";



        public _AllPay()
        {
            //getkey();
        }
        public _AllPay(AllInOne oPay)
        {

            //getkey();
        }



        public string PayOfOrder(AllPayModel pay)
        {


            List<string> enErrors = new List<string>();
            using (AllInOne oPayment = new AllInOne())
            {
                /* 服務參數 */
                oPayment.ServiceMethod = HttpMethod.HttpPOST;
                oPayment.ServiceURL = ServiceURL;
                oPayment.HashKey = HashKey;
                oPayment.HashIV = HashIV;

                oPayment.MerchantID = MerchantID;
                /* 基本參數 */
                //您要收到付款完成通知的伺服器端網址
                oPayment.Send.ReturnURL = string.IsNullOrEmpty(pay.ReturnURL) ? ReturnURL : pay.ReturnURL;
                //oPayment.Send.ClientBackURL = "https://developers.allpay.com.tw/AioMock/MerchantClientBackUrl";
                //您要歐付寶返回按鈕導向的瀏覽器端網址
                oPayment.Send.ClientBackURL = pay.ClientBackURL;
                //您要收到付款完成通知的瀏覽器端網址>
                oPayment.Send.OrderResultURL = pay.OrderResultURL;
                //oPayment.Send.OrderResultURL = ReturnURL;

                oPayment.Send.MerchantTradeNo = pay.MerchantTradeNo;
                oPayment.Send.MerchantTradeDate = DateTime.Now;
                oPayment.Send.TotalAmount = pay.TotalAmount;
                oPayment.Send.TradeDesc = pay.TradeDesc;
                oPayment.Send.ChoosePayment = PaymentMethod.ALL;
                oPayment.Send.Remark = pay.Remark;
                oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;
                oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.Yes;
                oPayment.Send.HoldTrade = HoldTradeType.No;
                oPayment.Send.DeviceSource = DeviceType.PC;
                oPayment.Send.IgnorePayment = "Tenpay";

                // 加入選購商品資料。
                foreach (var item in pay.Items)
                {
                    oPayment.Send.Items.Add(new Item() { Name = item.Name, Price = item.Price, Currency = item.Currency, Quantity = item.Quantity, URL = item.URL });
                }


                /* Credit 分期延伸參數 */

                oPayment.SendExtend.CreditInstallment = pay.CreditInstallment;
                oPayment.SendExtend.InstallmentAmount = pay.InstallmentAmount;
                //是否使用紅利折抵
                oPayment.SendExtend.Redeem = false;
                //是否使用銀聯卡
                oPayment.SendExtend.UnionPay = false;
                /* 產生訂單 */
                enErrors.AddRange(oPayment.CheckOut());
                /* 產生產生訂單 Html Code 的方法 */
                string szHtml = String.Empty;
                enErrors.AddRange(oPayment.CheckOutString(ref szHtml));
                HtmlCode = szHtml;
            }

            return HtmlCode;
        }


        public void PayOfFeedBack(string MerchantTradeNo)
        {
            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;

            

            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    /* 服務參數 */
                    oPayment.ServiceMethod = HttpMethod.ServerPOST; // 或使用 HttpMethod.HttpSOAP;
                    oPayment.ServiceURL = "https://payment.allpay.com.tw/Cashier/QueryTradeInfo/V2";
                    oPayment.HashKey = HashKey;
                    oPayment.HashIV = HashIV;
                    oPayment.MerchantID = MerchantID;
                    /* 基本參數 */
                    oPayment.Query.MerchantTradeNo = MerchantTradeNo;
                    /* 查詢訂單 */
                    enErrors.AddRange(oPayment.QueryTradeInfo(ref htFeedback));
                }
                // 取回所有資料
                if (enErrors.Count() == 0)
                {
                    /* 查詢後的回傳的基本參數 */
                    string szMerchantID = String.Empty;
                    string szMerchantTradeNo = String.Empty;
                    string szTradeNo = String.Empty;
                    string szTradeAmt = String.Empty;
                    string szPaymentDate = String.Empty;
                    string szPaymentType = String.Empty;
                    string szHandlingCharge = String.Empty;
                    string szPaymentTypeChargeFee = String.Empty;
                    string szTradeDate = String.Empty;
                    string szTradeStatus = String.Empty;

                    string szItemName = String.Empty;
                    /* 使用 WebATM 交易時，回傳的額外參數 */
                    string szWebATMAccBank = String.Empty;
                    string szWebATMAccNo = String.Empty;
                    /* 使用 ATM 交易時，回傳的額外參數 */
                    string szATMAccBank = String.Empty;
                    string szATMAccNo = String.Empty;
                    /* 使用 CVS 交易時，回傳的額外參數 */
                    string szPaymentNo = String.Empty;
                    string szPayFrom = String.Empty;
                    /* 使用 Tenpay 交易時，回傳的額外參數 */
                    string szTenpayTradeNo = String.Empty;
                    /* 使用 Credit 交易時，回傳的額外參數 */
                    string szGwsr = String.Empty;
                    string szProcessDate = String.Empty;
                    string szAuthCode = String.Empty;
                    string szAmount = String.Empty;
                    string szStage = String.Empty;
                    string szStast = String.Empty;
                    string szStaed = String.Empty;
                    string szECI = String.Empty;
                    string szCard4No = String.Empty;
                    string szCard6No = String.Empty;
                    string szRedDan = String.Empty;
                    string szRedDeAmt = String.Empty;
                    string szRedOkAmt = String.Empty;
                    string szRedYet = String.Empty;
                    string szPeriodType = String.Empty;
                    string szFrequency = String.Empty;
                    string szExecTimes = String.Empty;
                    string szPeriodAmount = String.Empty;
                    string szTotalSuccessTimes = String.Empty;
                    string szTotalSuccessAmount = String.Empty;
                    // 取得資料於畫面
                    foreach (string szKey in htFeedback.Keys)
                    {
                        switch (szKey)
                        {

                            /* 查詢後的回傳的基本參數 */
                            case "MerchantID": szMerchantID = htFeedback[szKey].ToString(); break;
                            case "MerchantTradeNo": szMerchantTradeNo = htFeedback[szKey].ToString(); break;
                            case "TradeNo": szTradeNo = htFeedback[szKey].ToString(); break;
                            case "TradeAmt": szTradeAmt = htFeedback[szKey].ToString(); break;
                            case "PaymentDate": szPaymentDate = htFeedback[szKey].ToString(); break;
                            case "PaymentType": szPaymentType = htFeedback[szKey].ToString(); break;
                            case "HandlingCharge": szHandlingCharge = htFeedback[szKey].ToString(); break;
                            case "PaymentTypeChargeFee": szPaymentTypeChargeFee = htFeedback[szKey].ToString(); break;
                            case "TradeDate": szTradeDate = htFeedback[szKey].ToString(); break;
                            case "TradeStatus": szTradeStatus = htFeedback[szKey].ToString(); break;
                            case "ItemName": szItemName = htFeedback[szKey].ToString(); break;
                            /* 使用 WebATM 交易時回傳的參數 */
                            case "WebATMAccBank": szWebATMAccBank = htFeedback[szKey].ToString(); break;
                            case "WebATMAccNo": szWebATMAccNo = htFeedback[szKey].ToString(); break;
                            /* 使用 ATM 交易時回傳的參數 */
                            case "ATMAccBank": szATMAccBank = htFeedback[szKey].ToString(); break;
                            case "ATMAccNo": szATMAccNo = htFeedback[szKey].ToString(); break;
                            /* 使用 CVS 交易時回傳的參數 */
                            case "PaymentNo": szPaymentNo = htFeedback[szKey].ToString(); break;
                            case "PayFrom": szPayFrom = htFeedback[szKey].ToString(); break;
                            /* 使用 Tenpay 交易時回傳的參數 */
                            case "TenpayTradeNo": szTenpayTradeNo = htFeedback[szKey].ToString(); break;
                            /* 使用 Credit 交易時回傳的參數 */
                            case "gwsr": szGwsr = htFeedback[szKey].ToString(); break;
                            case "process_date": szProcessDate = htFeedback[szKey].ToString(); break;
                            case "auth_code": szAuthCode = htFeedback[szKey].ToString(); break;
                            case "amount": szAmount = htFeedback[szKey].ToString(); break;
                            case "stage": szStage = htFeedback[szKey].ToString(); break;
                            case "stast": szStast = htFeedback[szKey].ToString(); break;
                            case "staed": szStaed = htFeedback[szKey].ToString(); break;
                            case "eci": szECI = htFeedback[szKey].ToString(); break;
                            case "card4no": szCard4No = htFeedback[szKey].ToString(); break;
                            case "card6no": szCard6No = htFeedback[szKey].ToString(); break;
                            case "red_dan": szRedDan = htFeedback[szKey].ToString(); break;
                            case "red_de_amt": szRedDeAmt = htFeedback[szKey].ToString(); break;

                            case "red_ok_amt": szRedOkAmt = htFeedback[szKey].ToString(); break;
                            case "red_yet": szRedYet = htFeedback[szKey].ToString(); break;
                            case "PeriodType": szPeriodType = htFeedback[szKey].ToString(); break;
                            case "Frequency": szFrequency = htFeedback[szKey].ToString(); break;
                            case "ExecTimes": szExecTimes = htFeedback[szKey].ToString(); break;
                            case "PeriodAmount": szPeriodAmount = htFeedback[szKey].ToString(); break;
                            case "TotalSuccessTimes": szTotalSuccessTimes = htFeedback[szKey].ToString(); break;
                            case "TotalSuccessAmount": szTotalSuccessAmount = htFeedback[szKey].ToString(); break;
                            default: break;
                        }


                    }

                    using (var db = new MyAnythingEntities())
                    {
                        var order = db.OrderMaster.Where(o => o.MerchantOrderNo == MerchantTradeNo).FirstOrDefault();
                        if (order != null)
                        {
                            order.TradeNo = szTradeNo;
                            order.TradeStatus = szTradeStatus;
                            order.PaymentType = szPaymentType;
                            db.SaveChanges();
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                // 顯示錯誤訊息。
                if (enErrors.Count() > 0)
                {
                    string szErrorMessage = String.Join("\\r\\n", enErrors);
                }
            }
        }

        public SelectList GetPayMethods()
        {
            var EntityState = new SelectList(Enum.GetValues(typeof(EntityState)).Cast<EntityState>().Select(v => new SelectListItem
                 {
                     Text = v.ToString(),
                     Value = ((int)v).ToString()
                 }).ToList(), "Value", "Text");
            return EntityState;
        }

        public enum PayMethod
        {
            Credit = 0,
            Installment,
            ATM,
            CVS,

        }
    }


}