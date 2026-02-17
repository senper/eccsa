using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECCSA.BackOffice.Server.Persistence.Repositories
{
    public static class Validation
    {

        public static bool IsValid(ContactTypeEnum type, string address)
        {
            if (string.IsNullOrEmpty(address))
                return false;
            return type switch
            {
                ContactTypeEnum.NotSet => false,
                ContactTypeEnum.Email => IsEmail(address),
                ContactTypeEnum.Mobile or ContactTypeEnum.Landline or ContactTypeEnum.Fax => IsPhoneNumber(address),
                ContactTypeEnum.Website => IsWebsite(address),
                ContactTypeEnum.Facebook => IsFacebook(address),
                ContactTypeEnum.Twitter => IsTwitter(address),
                _ => true,
            };
        }
        public static bool IsPhoneNumber(string input)
        {
            //251 911 630 791
            if (string.IsNullOrEmpty(input)) return false;
            if (input.Length < 9 || input.Length > 20) return false;

            var address = input.Replace(" ", "").Replace("-", "").Replace("+", "").Replace("(", "").Replace(")", "");
            return Regex.IsMatch(address, "^[0-9]{12}$")
                   || Regex.IsMatch(address, "^[0-9]{11}$")
                   || Regex.IsMatch(address, "^[0-9]{10}$");

        }

        public static bool IsTIN(string input)
        {
            //251 911 630 791
            if (string.IsNullOrEmpty(input)) return false;
            if (input.Length < 10 || input.Length > 11) return false;

            var address = input.Replace(" ", "").Replace("-", "").Replace("+", "").Replace("(", "").Replace(")", "");
            return Regex.IsMatch(address, "^[0-9]{12}$")
                   || Regex.IsMatch(address, "^[0-9]{11}$")
                   || Regex.IsMatch(address, "^[0-9]{10}$");

        }

        public static bool IsEmail(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            //a@b.et
            if (input.Length < 6 || input.Length > 50) return false;
            if (!input.Contains("@") || !input.Contains(".")) return false;
            try
            {
                var addr = new MailAddress(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsWebsite(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            //www.a.et
            if (input.Length < 9 || input.Length > 60) return false;

            bool result = Uri.TryCreate(input, UriKind.Absolute, out Uri uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!result && !input.StartsWith("http"))
            {
                var tmp = $"http://{input}";
                result = Uri.TryCreate(tmp, UriKind.Absolute, out uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            }
            return result;
        }
        public static bool IsFacebook(string input)
        {
            if (!IsWebsite(input)) return false;
            return Regex.IsMatch(input, @"(?:(?:http|https):\/\/)?(?:www.)?facebook.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-]*\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?");

        }
        public static bool IsTwitter(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            if (!IsWebsite(input)) return false;
            return true;
        }
    }
}