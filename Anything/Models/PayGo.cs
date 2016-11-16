using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Models
{
    public class PayGoRequest
    {
        public string MerchantID { get;set;}
        public string RespondType { get; set; }
        public string CheckValue { get; set; }
        public string TimeStamp { get; set; }
        public string Version { get; set; }
        public string LangType { get; set; }
        public string MerchantOrderNo { get; set; }
        public int Amt { get; set; }
        public string ItemDesc { get; set; }
        public int TradeLimit { get; set; }

        public string ExpireDate { get; set; }
        public string ExpireTime { get; set; }

        public string ReturnURL { get { return System.Configuration.ConfigurationManager.AppSettings["ReturnURL"];} set; }
        public string NotifyURL { get { return System.Configuration.ConfigurationManager.AppSettings["NotifyURL"]; } set; }
        public string CustomerURL { get; set; }
        public string ClientBackUrl { get; set; }
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
        public string MerchantID { get; set; }
        public int Amt { get; set; }
        public string TradeNo { get; set; }
        public string MerchantOrderNo { get; set; }
        public string PaymentType { get; set; }

        public string RespondType { get; set; }
        public string CheckCode { get; set; }
        public DateTime PayTime { get; set; }
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
        public string PayBankCode { get; set; }
        public string PayerAccount5Code { get; set; }
        public string CoedNo { get; set; }
        public string Barcode1 { get; set; }
        public string Barcode2 { get; set; }
        public string Barcode3 { get; set; }
    }
}