using System.Net.Http;
using System.Threading.Tasks;

namespace ClimaTempoAPI.Interfaces
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);

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
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);
    }
}