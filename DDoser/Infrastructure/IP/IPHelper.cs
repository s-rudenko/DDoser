using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DDoser.Infrastructure.IP
{
    public class IPHelper
    {
        public static string GetMyCountryByIp()
        {
            try
            {
                var httpClient = new HttpClient();
                var ip = httpClient.GetStringAsync("https://api.ipify.org").GetAwaiter().GetResult();

                var info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                var el = System.Text.Json.JsonDocument.Parse(info).RootElement;
                return el.GetProperty("country").ToString() + el.GetProperty("city").ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
