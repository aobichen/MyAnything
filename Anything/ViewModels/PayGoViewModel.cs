using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Anything.ViewModels
{
    public class Pay2Go
    {
//        1. 排列參數並串聯
//Amt=200&MerchantID=123456&MerchantOrderNo=20140901001&TimeStamp=140
//3243286&Version=1.1
//2. 前後加上商店專屬的 HashKey 及 HashIV
//HashKey=1A3S21DAS3D1AS65D1&Amt=200&MerchantID=123456&MerchantOrd
//erNo=20140901001&TimeStamp=1403243286&Version=1.1&HashIV=1AS56D1AS
//24D
//3. 使用 SHA256 壓碼過後並轉大寫
//strtoupper(hash("sha256", $CheckValue_str));

        public string CheckValue(int Amt, string MerchantOrderNo, string TimeStamp)
        {
            var pay = new PayGoRequest();
            var HashKey = System.Configuration.ConfigurationManager.AppSettings["PayHashKey"];
            var HashIV = System.Configuration.ConfigurationManager.AppSettings["PayHashIV"];

            var text = string.Format(@"HashKey={0}&Amt={1}&MerchantID={2}&MerchantOrderNo={3}&TimeStamp={4}&Version={5}&HashIV={6}"
                , HashKey, Amt, pay.MerchantID, MerchantOrderNo, TimeStamp, pay.Version, HashIV);

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }

            return hashString.ToUpper();
        }
    }
    public class PayGoRequest
    {
        public string MerchantID { get { return System.Configuration.ConfigurationManager.AppSettings["MerchantID"]; } }
        public string RespondType { get; set; }
        public string CheckValue { get; set; }
        public string TimeStamp { get; set; }
        public string Version { get { return System.Configuration.ConfigurationManager.AppSettings["PayGoVersion"]; } }
        public string LangType { get; set; }
        public string MerchantOrderNo { get; set; }
        public int Amt { get; set; }
        public string ItemDesc { get; set; }
        public int TradeLimit { get; set; }

        public string ExpireDate { get; set; }
        public string ExpireTime { get; set; }

        public string ReturnURL { get { return System.Configuration.ConfigurationManager.AppSettings["ReturnURL"];}  }
        public string NotifyURL { get { return System.Configuration.ConfigurationManager.AppSettings["NotifyURL"]; }  }
        public string CustomerURL { get { return System.Configuration.ConfigurationManager.AppSettings["CustomerURL"]; } }
        public string ClientBackUrl { get { return System.Configuration.ConfigurationManager.AppSettings["ClientBackUrl"]; } }
        public string Email { get; set; }
        public int EmailModify { get; set; }
        public int LoginType { get; set; }
        public string OrderComment { get; set; }
        public int CREDIT { get; set; }
        public int CreditRed { get; set; }
        public string InstFlag { get; set; }
        public int UNIONPAY { get; set; }
        public int WEBATM { get; set; }
        public int VACC { get; set; }
        public int CVS { get; set; }
        public int BARCODE { get; set; }
        public int CUSTOMER { get; set; }

        
    }

    
    public class PayGoRespond
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }
        
    }

    //[Serializable]
    public class PayResult
    {
        public string MerchantID { get; set; }
        public int Amt { get; set; }
        public string TradeNo { get; set; }
        public string MerchantOrderNo { get; set; }
        public string PaymentType { get; set; }

        public string RespondType { get; set; }
        public string CheckCode { get; set; }

        public string ExpireTime { get; set; }
        public string ExpireDate { get; set; }
        public string PayTime { get; set; }
        public string IP { get; set; }
        public string EscrowBank { get; set; }
        public string Auth { get; set; }
        public string Card6No { get; set; }
        public string Card4No { get; set; }
        public int Inst { get; set; }
        public int InstFirst { get; set; }
        public int InstEach { get; set; }
        public string ECI { get; set; }
        public int TokenUseStatus { get; set; }
        public int RedAmt { get; set; }
        public string BankCode { get; set; }
        public string PayerAccount5Code { get; set; }
        public string CodeNo { get; set; }
        public string Barcode1 { get; set; }
        public string Barcode2 { get; set; }
        public string Barcode3 { get; set; }

       
        public string RespondCode { get; set; }
    }
}