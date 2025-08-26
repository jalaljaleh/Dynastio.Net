using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    /// <summary>
    /// Lightweight HTTP client for Dynastio API calls.
    /// Handles JSON serialization, default headers, and error checking.
    /// </summary>
    internal sealed class ApiHttpClient : IDisposable
    {
        private static readonly MediaTypeWithQualityHeaderValue JsonMediaType = new MediaTypeWithQualityHeaderValue("application/json");
        private readonly HttpClient _http;
        private readonly JsonSerializerSettings _jsonSettings;

        /// <summary>
        /// Initializes a new instance with bearer authentication, GZip/Deflate decompression,
        /// a custom User-Agent header, and a per-request timeout.
        /// </summary>
        /// <param name="tokenKey">Auth header name (e.g. "Authorization").</param>
        /// <param name="tokenValue">Auth token value.</param>
        /// <param name="userAgent">Value for the User-Agent header.</param>
        /// <param name="timeout">HttpClient timeout for each request.</param>
        public ApiHttpClient(string tokenKey, string tokenValue, string userAgent, TimeSpan timeout)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseDefaultCredentials = false
            };

            _http = new HttpClient(handler, disposeHandler: true)
            {
                Timeout = timeout
            };

            // Bearer auth in a single header
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", $"{tokenKey}:{tokenValue}");
            _http.DefaultRequestHeaders.Add(tokenKey, tokenValue);

            // Standard headers
            _http.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            _http.DefaultRequestHeaders.Accept.Add(JsonMediaType);

            // JSON: camelCase, no automatic date parsing overhead
            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateParseHandling = DateParseHandling.None
            };
        }

        /// <summary>
        /// Issues a GET request and returns the raw response body.
        /// </summary>
        /// <param name="uri">Full request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<string> GetStringAsync(
            string uri,
            CancellationToken cancellationToken = default)
        {
            using var res = await _http.GetAsync(uri, cancellationToken)
                                       .ConfigureAwait(false);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync()
                          .ConfigureAwait(false);
        }

        /// <summary>
        /// Issues a GET request and deserializes the JSON response to T.
        /// </summary>
        public async Task<T> GetJsonAsync<T>(
            string uri,
            CancellationToken cancellationToken = default)
        {
            var json = await GetStringAsync(uri, cancellationToken)
                         .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json, _jsonSettings)!;
        }

        /// <summary>
        /// Issues a POST request with a JSON body and returns the raw response body.
        /// </summary>
        public async Task<string> PostStringAsync(
            string uri,
            object payload,
            CancellationToken cancellationToken = default)
        {
            using var content = Serialize(payload);
            using var res = await _http.PostAsync(uri, content, cancellationToken)
                                           .ConfigureAwait(false);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync()
                          .ConfigureAwait(false);
        }

        /// <summary>
        /// Issues a POST request with a JSON body and deserializes the JSON response to T.
        /// </summary>
        public async Task<T> PostJsonAsync<T>(
            string uri,
            object payload,
            CancellationToken cancellationToken = default)
        {
            var json = await PostStringAsync(uri, payload, cancellationToken)
                         .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json, _jsonSettings)!;
        }

        /// <summary>
        /// Issues a PUT request with a JSON body and returns the raw response body.
        /// </summary>
        public async Task<string> PutStringAsync(
            string uri,
            object payload,
            CancellationToken cancellationToken = default)
        {
            using var content = Serialize(payload);
            using var res = await _http.PutAsync(uri, content, cancellationToken)
                                           .ConfigureAwait(false);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync()
                          .ConfigureAwait(false);
        }

        /// <summary>
        /// Issues a DELETE request and returns the raw response body.
        /// </summary>
        public async Task<string> DeleteStringAsync(
            string uri,
            CancellationToken cancellationToken = default)
        {
            using var res = await _http.DeleteAsync(uri, cancellationToken)
                                       .ConfigureAwait(false);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync()
                          .ConfigureAwait(false);
        }

        /// <summary>
        /// Releases the underlying HttpClient.
        /// </summary>
        public void Dispose() => _http.Dispose();


        #region Private Helpers

        private StringContent Serialize(object payload)
        {
            var json = JsonConvert.SerializeObject(payload, _jsonSettings);
            return new StringContent(json, Encoding.UTF8, JsonMediaType.MediaType);
        }

        #endregion
    }
}
