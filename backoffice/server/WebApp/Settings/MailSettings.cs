using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Settings
{
   public class SmsService
    {
        public static async Task<bool> Send(string mobileNo, String message)
        {
            var vaidatedPhone = SanitizePhone(mobileNo);
            if(!vaidatedPhone.IsValid)
                return false;
            mobileNo = vaidatedPhone.Phone;
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.geezsms.com/api/v1/sms/send");
            var content = new MultipartFormDataContent();
            content.Add(new StringContent("AC8bezOncgwEiMH7vVgSKgJxNShlERwu"), "token");
            content.Add(new StringContent(mobileNo), "phone");
            content.Add(new StringContent(message), "msg");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return true;
        }

        private static (string Phone, bool IsValid) SanitizePhone(string phone)
        {
           var ph = phone.Replace("+", "").Replace(" ", "").Trim();
            if(ph.StartsWith("09"))
                ph = "2519" + ph.Substring(2,ph.Length - 2);

            //251 911 630 791
            if (ph.Length != 12)
                return (phone, false);
            var charArray = ph.ToCharArray();
            foreach (var c in charArray)
            {
                if(!Char.IsNumber(c))
                    return (phone, false);
            }
            return (ph, true);
        }
    }
}
