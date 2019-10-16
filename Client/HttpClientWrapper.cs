
using ClimaTempoAPI.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClimaTempoAPI.Client
{
    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        ///  Send a GET request to the specified Uri and return the response body as a string
        ///  in an asynchronous operation.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
            => await _httpClient.GetAsync(requestUri);
    }
}
