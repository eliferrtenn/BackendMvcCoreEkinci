using Ekinci.Common.Caching;
using System.Text.RegularExpressions;

namespace Ekinci.Common.Extentions
{
    public static class StringExtensions
    {
        public static AppSettingsKeys _appSettingsKeys;
        public static bool IsNullOrEmtpy(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNull(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static string ToEmptyStringIndicator(this string str)
        {
            return string.IsNullOrEmpty(str) ? "--" : str;
        }

        public static string ReplaceENTER(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return str.Replace(Environment.NewLine, "<br/>");
        }

        public static string ToStringData(this object str)
        {
            return str?.ToString() ?? string.Empty;
        }

        public static string ToPhoneNumber(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            else if (str.Length == 10)
            {
                return Regex.Replace(str, @"^\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*$", "0 ($1$2$3) $4$5$6 $7$8 $9$10");
            }
            else if (str.Length == 11)
            {
                return Regex.Replace(str, @"^\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*$", "$1 ($2$3$4) $5$6$7 $8$9 $10$11");
            }
            else if (str.Length == 12)
            {
                return Regex.Replace(str, @"^\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*(\d)\D*$", "+$1$2 ($3$4$5) $6$7$8 $9$10 $11$12");
            }
            else
            {
                return str;
            }
        }
        public static string PrepareCDNUrl(this string imageUrl, string folder)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return imageUrl;
            }

            return string.Format("{0}{1}{2}", "https://ekinciapp.b-cdn.net/", folder, imageUrl);
        }
    }
}
