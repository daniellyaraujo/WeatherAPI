using ClimaTempoAPI.Interfaces;
using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using ClimaTempoAPI.Models.Days;
using ClimaTempoAPI.Models.Hour;
using System;
using System.Net.Http;
using WeatherAPI.Models.Region;

namespace ClimaTempoAPI.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClient _client;

        /// <summary>
        /// 
        /// </summary>
        public WeatherService(IHttpClient client)
        {
            _client = client;
        }

        public RegionResponse GetWeatherByRegion(string region)
        {
            RegionResponse response;

            var result = _client.GetAsync("http://apiadvisor.climatempo.com.br/api/v1/forecast/region/" + region + "?token=0b5e7fb3c07b2dbcbf28773b0138e85d").GetAwaiter().GetResult();

            if (result == null) return null;

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                response = result.Content.ReadAsAsync<RegionResponse>().GetAwaiter().GetResult();

                return response;
            }

            if (!result.IsSuccessStatusCode)
            {
                response = new RegionResponse { StatusCode = result.StatusCode };

                return response;
            }

            response = result.Content.ReadAsAsync<RegionResponse>().Result;

            return response;
        }

        public CityResponse GetCurrentWeatherByCity(ParameterRequest city)
        {
            throw new NotImplementedException();
        }

        public HourResponse Get72hrWeatherById(ParameterRequest city)
        {
            throw new NotImplementedException();
        }

        public DaysResponse Get15DaysWeather(ParameterRequest city)
        {
            throw new NotImplementedException();
        }
    }
}
