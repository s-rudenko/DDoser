using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DDoser.Infrastructure.QueryString.Modifiers
{
    public class RandomStringHelper : IModifier
    {
        public static string Ancor = "RandomString";

        public string Modify(string url)
        {
            return url.Replace(Ancor, WebUtility.UrlEncode(RandomString(3)));
        }
        private static string RandomString(int length)
        {
            const string chars ="йцукенгшщзхфвапролджєячсмитьбюЙЦУКЕНГШЩЗЖДЛОРПАВФЯЧСМИТЬБЮ";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
