//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Anything.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PayGo
    {
        public int ID { get; set; }
        public string MerchantOrderNo { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string MerchantID { get; set; }
        public int Amt { get; set; }
        public string TradeNo { get; set; }
        public Nullable<System.DateTime> PayTime { get; set; }
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
        public string RespondType { get; set; }
        public string PaymentType { get; set; }
    }
}
