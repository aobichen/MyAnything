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
    
    public partial class AllPayFeed
    {
        public int ID { get; set; }
        public string MerchantID { get; set; }
        public string MerchantTradeNo { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTypeChargeFee { get; set; }
        public string SimulatePaid { get; set; }
        public string TradeAmt { get; set; }
        public string TradeDate { get; set; }
        public string TradeNo { get; set; }
        public string RtnCode { get; set; }
        public string RtnMsg { get; set; }
    }
}
