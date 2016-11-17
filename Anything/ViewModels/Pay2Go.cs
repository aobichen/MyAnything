using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.ViewModels
{
    public class PayGoRequest
    {
        public string MerchantID { get; set; }
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

        public string ReturnURL { get { return System.Configuration.ConfigurationManager.AppSettings["ReturnURL"]; } set; }
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

    public class Pay2GoResponsd
    {

        public int ID { get; set; }
        public string MerchantOrderNo { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string MerchantID { get; set; }
        public int Amt { get; set; }
        public string TradeNo { get; set; }
        public System.DateTime PayTime { get; set; }
        public string IP { get; set; }
        public string EscrowBank { get; set; }
        public string RespondCode { get; set; }
        public string Auth { get; set; }
        public string Card6No { get; set; }
        public string Card4No { get; set; }
        public Nullable<int> Inst { get; set; }
        public Nullable<int> InstFirst { get; set; }
        public Nullable<int> InstEach { get; set; }
        public Nullable<int> TokenUseStatus { get; set; }
        public Nullable<int> RedAmt { get; set; }
        public string PayBankCode { get; set; }
        public string PayAccount5Code { get; set; }
        public string CodeNo { get; set; }
        public string Barcode_1 { get; set; }
        public string Barcode_2 { get; set; }
        public string Barcode_3 { get; set; }

        public string RespondCode { get; set; }
    
    }
}