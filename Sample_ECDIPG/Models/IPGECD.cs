using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sample_ECDIPG.Models
{
    public class IPGECD
    {


        public  const string PayStart_url = "https://ecd.shaparak.ir/ipg_ecd/PayStart";
        private const string PayRequest_url = "https://ecd.shaparak.ir/ipg_ecd/PayRequest";
        private const string PayConfirmation_url = "https://ecd.shaparak.ir/ipg_ecd/PayConfirmation";
        private const string PayReverse_url = "https://ecd.shaparak.ir/ipg_ecd/PayReverse";
        

        public IPGResult PayRequest(string Key, string TerminalNumber, string BuyID, long Amount, string Date, string Time, string RedirectURL, string Language)
        {
            IPGResult Result = new IPGResult();
            try
            {

                string Checksum = HashSHa1(TerminalNumber + BuyID + Amount.ToString() + Date + Time + RedirectURL + Key);

                var model_Send = new { TerminalNumber = TerminalNumber, BuyID = BuyID, Amount = Amount, Date = Date, Time = Time, RedirectURL = RedirectURL, Language = Language, CheckSum = Checksum };



                var webClient = new WebClient();

                webClient.Encoding = System.Text.Encoding.UTF8;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json;charset=utf-8";
                var rawMessage = JsonConvert.SerializeObject(model_Send);

                rawMessage = webClient.UploadString(PayRequest_url, rawMessage);

                Result = JsonConvert.DeserializeObject<IPGResult>(rawMessage);
            
                return Result;
            }
            catch (Exception)
            {

                return Result;
            }
        }


        public IPGResult PayConfirmation(string Token)
        {
            IPGResult Result = new IPGResult();

            try
            {

                var model_Send = new { Token = Token };

                var webClient = new WebClient();

                webClient.Encoding = System.Text.Encoding.UTF8;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json;charset=utf-8";
                var rawMessage = JsonConvert.SerializeObject(model_Send);

                rawMessage = webClient.UploadString(PayConfirmation_url, rawMessage);

                Result = JsonConvert.DeserializeObject<IPGResult>(rawMessage);


                return Result;
            }
            catch (Exception)
            {

                return Result;
            }
        }


        public IPGResult PayReverse(string Token)
        {
            IPGResult Result = new IPGResult();

            try
            {
              
                var model_Send = new { Token = Token };

                var webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json;charset=utf-8";
                var rawMessage = JsonConvert.SerializeObject(model_Send);
                rawMessage = webClient.UploadString(PayReverse_url, rawMessage);
                Result = JsonConvert.DeserializeObject<IPGResult>(rawMessage);

                return Result;
            }
            catch (Exception)
            {

                return Result;
            }
        }



        #region Public Class
        public class IPGResult
        {
            public IPGResult()
            {
                this.ErrorCode = string.Empty;
                this.ErrorDescription = string.Empty;
                this.Res = string.Empty;
                this.State = 0;
            }

            public int State { get; set; }
            public string Res { get; set; }
            public string ErrorDescription { get; set; }
            public string ErrorCode { get; set; }
        }

        public class PayStartResult
        {
           

            public string Token { get; set; }
            public int State { get; set; }
            public string ErrorCode { get; set; }
            public string BuyID { get; set; }
            public string TrackingNumber { get; set; }
            public string ReferenceNumber { get; set; }
            public string ErrorDescription { get; set; }
            public long Amount { get; set; }
        }

        #endregion Public Class

        #region Private Function
        private string HashSHa1(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        #endregion Private Function

    }
}