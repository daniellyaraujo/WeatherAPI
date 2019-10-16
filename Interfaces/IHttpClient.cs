using System.Net.Http;
using System.Threading.Tasks;

namespace ClimaTempoAPI.Interfaces
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}