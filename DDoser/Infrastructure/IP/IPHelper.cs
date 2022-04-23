using Newtonsoft.Json;
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
            IpInfoModel ipInfo = new();
            try
            {
                var httpClient = new HttpClient();
                var ip = httpClient.GetStringAsync("https://api.ipify.org").GetAwaiter().GetResult();

                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfoModel>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            return ipInfo.Country;
        }
    }
}
