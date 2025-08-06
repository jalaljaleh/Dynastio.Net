using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    internal class ApiHttpClient : IDisposable
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _jsonSettings;

        public ApiHttpClient(string tokenKey, string tokenValue, string userAgent, TimeSpan timeout)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                UseDefaultCredentials = true
            };

            _client = new HttpClient(handler, false)
            {
                Timeout = timeout
            };

            //  _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(userAgent));
            _client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add(tokenKey, tokenValue);

            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public async Task<string> GetStringAsync(string url)
        {
            using var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<T> GetJsonAsync<T>(string url) where T : class, new()
        {
            var json = await GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
        }

        public async Task<string> PostStringAsync(string url, object body)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(body, _jsonSettings),
                Encoding.UTF8,
                "application/json"
            );

            using var response = await _client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<T> PostJsonAsync<T>(string url, object body) where T : class, new()
        {
            var json = await PostStringAsync(url, body);
            return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
        }

        public async Task<string> PutAsync(string url, object body)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(body, _jsonSettings),
                Encoding.UTF8,
                "application/json"
            );

            using var response = await _client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteAsync(string url)
        {
            using var response = await _client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
