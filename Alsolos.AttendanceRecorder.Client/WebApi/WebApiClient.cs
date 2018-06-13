using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NLog;

namespace Alsolos.AttendanceRecorder.Client.WebApi
{
    public class WebApiClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly Uri BaseAddress = new Uri("http://localhost:30515/");

        public async Task<T> GetAsync<T>(string relativeUrl)
        {
            using (var client = InitClient())
            {
                var response = await client.GetAsync(relativeUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<T>();
                    return data;
                }

                Logger.Error($"Getting data from '{relativeUrl} failed. {response.StatusCode} - {response.ReasonPhrase}");
                return default(T);
            }
        }

        private static HttpClient InitClient()
        {
            var client = new HttpClient
            {
                BaseAddress = BaseAddress,
                Timeout = TimeSpan.FromSeconds(10)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
