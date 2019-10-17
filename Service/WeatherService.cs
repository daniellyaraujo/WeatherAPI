using ClimaTempoAPI.Interfaces;
using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using ClimaTempoAPI.Models.Days;
using ClimaTempoAPI.Models.Hour;
using System;
using System.Net.Http;
using WeatherAPI.Models;
using WeatherAPI.Models.Region;

namespace ClimaTempoAPI.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClient _client;
        private string _host;
        private string _token;

        /// <summary>
        /// 
        /// </summary>
        public WeatherService(IHttpClient client)
        {
            _client = client;
            _host = "http://apiadvisor.climatempo.com.br/api/v1";
            _token = "token=0b5e7fb3c07b2dbcbf28773b0138e85d";
        }

        public RegionResponse GetWeatherByRegion(string region)
        {
            RegionResponse response;

            var result = _client.GetAsync($"{_host}/forecast/region/{region}?{_token}").GetAwaiter().GetResult();

            if (result == null) return null;

            if (!result.IsSuccessStatusCode)
            {
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    response = result.Content.ReadAsAsync<RegionResponse>().Result;
                    response.StatusCode = result.StatusCode;
                    response.Data = null;
                    return response;
                }

                response = new RegionResponse { StatusCode = result.StatusCode };
                return response;
            }

            response = result.Content.ReadAsAsync<RegionResponse>().Result;
            response.StatusCode = result.StatusCode;
            return response;
        }

        public CityResponse GetCurrentWeatherByCity(ParameterRequest city)
        {
            throw new NotImplementedException();
        }

        public HourResponse Get72hrWeatherById(ParameterRequest parameterRequest)
        {
            if (parameterRequest == null || parameterRequest.State == null || parameterRequest.City == null)
            {
                var errorResponse = new HourResponse() { Detail = "Parâmetros Inválidos" };
                return errorResponse;
            }

            throw new NotImplementedException();
        }

        public DaysResponse Get15DaysWeather(ParameterRequest city)
        {
            throw new NotImplementedException();
        }
    }
}
