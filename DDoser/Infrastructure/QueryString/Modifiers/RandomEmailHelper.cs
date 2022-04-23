using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DDoser.Infrastructure.QueryString.Modifiers
{
    class RandomEmailHelper : IModifier
    {
        public static string Ancor = "RandomEmail";

        public string Modify(string url)
        {
            return url.Replace(Ancor, WebUtility.UrlEncode(RandomEmail()));
        }
        private static string RandomEmail()
        {
            const string chars = "qwertyuiopsdfghjklxcvbnm-1234567890";
            return new string(Enumerable.Repeat(chars, new Random().Next(5, 20)).Select(s => s[new Random().Next(s.Length)]).ToArray()) + "@gmail.com";
        }
    }
}
