
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

              
        /// Summary:
        ///     Send a PUT request to the specified Uri as an asynchronous operation.
        ///
        /// Parameters:
        ///   requestUri:
        ///     The Uri the request is sent to.
        ///
        ///   content:
        ///     The HTTP request content sent to the server.
        ///
        /// Returns:
        ///     The task object representing the asynchronous operation.
        ///
        /// Exceptions:
        ///   T:System.ArgumentNullException:
        ///     The requestUri is null.
        ///
        ///   T:System.Net.Http.HttpRequestException:
        ///     The request failed due to an underlying issue such as network connectivity, DNS
        ///     failure, server certificate validation or timeout.
        public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
              => await _httpClient.PutAsync(requestUri, content);

    }
}
