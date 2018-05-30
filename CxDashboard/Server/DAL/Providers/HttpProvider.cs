using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Providers
{
    public class HttpProvider
    {
        public static async Task<string> GetHttpRequest(string url)
        {
            var handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["TfsBaseUrl"]);

                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
            }
        }

        public static async Task<string> PostHttpRequest(string url, string jsonBody)
        {
            var handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["TfsBaseUrl"]);
                var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                using (var response = client.PostAsync(url, body))
                {
                    return await response.Result.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
