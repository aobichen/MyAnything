using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Configuration;
using System.Net;
using System.IO;

namespace Anything.ViewModels
{
    public class Pay2Go
    {

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

        public string ReturnURL { get { return System.Configuration.ConfigurationManager.AppSettings["ReturnURL"]; } }
        public string NotifyURL { get { return System.Configuration.ConfigurationManager.AppSettings["NotifyURL"]; } }
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

    [Serializable]
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
        public string PayBankCode { get; set; }
        public string BankCode { get; set; }
        public string PayerAccount5Code { get; set; }
        public string CodeNo { get; set; }
        public string Barcode_1 { get; set; }
        public string Barcode_2 { get; set; }
        public string Barcode_3 { get; set; }


        public string RespondCode { get; set; }
    }

    public class CreditCardStatus
    {
        public string MerchantID_ { get; set; }
        public string PostData_ { get; set; }
        public string RespondType { get; set; }
        public string Version { get; set; }
        public int Amt { get; set; }
        public string MerchantOrderNo { get; set; }

        public string TimeStamp { get; set; }

        /// <summary>
        /// 1使用訂單編號;2使用交易序號
        /// </summary>
        public int IndexType { get; set; }
        public string TradeNo { get; set; }

        /// <summary>
        /// 請款1;退款2
        /// </summary>
        public int CloseType { get; set; }

        public void Post()
        {
            MerchantID_ = ConfigurationManager.AppSettings["MerchantID"];
            var url = ConfigurationManager.AppSettings["CreditCareStatus"];
            var version = ConfigurationManager.AppSettings["CreditCareStatusVersion"];

            var result = string.Format("RespondType={0}&Version={1}&Amt={2}&MerchantOrder={3}&TimeStamp={4}&IndexType={5}&TradeNo={6}&CloseType={7}"
                , RespondType, version, Amt, MerchantOrderNo, TimeStamp, IndexType, TradeNo, CloseType);
            var AES = new CreditEncrypt().EncryptAES256(result);
            var Param = string.Format("MerchantID_={0}&PostData_={1}", MerchantID_, AES);
            var data = Encoding.ASCII.GetBytes(Param);
            HttpWebRequest Req = (HttpWebRequest)HttpWebRequest.Create(url);
            Req.Method = "POST";
            Req.ContentType = "application/x-www-form-urlencode;charset=utf8";
            Req.ContentLength = data.Length;
            using (Stream reqStream = Req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)Req.GetResponse();
            var a = response.Headers["JSONData"];
            var b = response.GetResponseHeader("JSONData");
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }

    public class CreditCardStatusResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }
    }

    public class CreditCardStatusResult
    {
        public string MerchantID { get; set; }
        public int Amt { get; set; }
        public string TradeNo { get; set; }

        public string MerchantOrderNo { get; set; }
    }

    public class CreditEncrypt
    {
        private string _sSecretKey = ConfigurationManager.AppSettings["PayHashKey"];
        private string _iv = ConfigurationManager.AppSettings["PayHashIV"];
        public string EncryptAES256(string source)//加密
        {
            string sSecretKey = _sSecretKey;
            string iv = _iv;
            byte[] sourceBytes = AddPKCS7Padding(Encoding.UTF8.GetBytes(source), 32);
            var aes = new RijndaelManaged();
            aes.Key = Encoding.UTF8.GetBytes(sSecretKey);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            ICryptoTransform transform = aes.CreateEncryptor();
            return ByteArrayToHex(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length)).ToLower();
        }

        public string DecryptAES256(string encryptData)//解密
        {
            string sSecretKey = _sSecretKey;
            string iv = _iv;
            var encryptBytes = HexStringToByteArray(encryptData.ToUpper());
            var aes = new RijndaelManaged();
            aes.Key = Encoding.UTF8.GetBytes(sSecretKey);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            ICryptoTransform transform = aes.CreateDecryptor();
            return Encoding.UTF8.GetString(RemovePKCS7Padding(transform.TransformFinalBlock(encryptBytes, 0, encryptBytes.Length)));

        }
        private static byte[] AddPKCS7Padding(byte[] data, int iBlockSize)
        {
            int iLength = data.Length;
            byte cPadding = (byte)(iBlockSize - (iLength % iBlockSize));
            var output = new byte[iLength + cPadding];
            Buffer.BlockCopy(data, 0, output, 0, iLength);
            for (var i = iLength; i < output.Length; i++)
                output[i] = (byte)cPadding;
            return output;
        }
        private static byte[] RemovePKCS7Padding(byte[] data)
        {
            int iLength = data[data.Length - 1];
            var output = new byte[data.Length - iLength];
            Buffer.BlockCopy(data, 0, output, 0, output.Length);
            return output;
        }
        private static string ByteArrayToHex(byte[] barray)
        {
            char[] c = new char[barray.Length * 2];
            byte b;
            for (int i = 0; i < barray.Length; ++i)
            {
                b = ((byte)(barray[i] >> 4));
                c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(barray[i] & 0xF));
                c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }
        private static byte[] HexStringToByteArray(string hexString)
        {

            int hexStringLength = hexString.Length;
            byte[] b = new byte[hexStringLength / 2];
            for (int i = 0; i < hexStringLength; i += 2)
            {
                int topChar = (hexString[i] > 0x40 ? hexString[i] - 0x37 : hexString[i] - 0x30) << 4;
                int bottomChar = hexString[i + 1] > 0x40 ? hexString[i + 1] - 0x37 : hexString[i + 1] - 0x30;
                b[i / 2] = Convert.ToByte(topChar + bottomChar);
            }
            return b;
        }

    }
}