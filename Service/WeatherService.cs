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
        private readonly IHttpClient _httpClient;
        private string _host;
        private string _token;

        /// <summary>
        /// 
        /// </summary>
        public WeatherService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
            _host = "http://apiadvisor.climatempo.com.br/api/v1";
            _token = "token=0b5e7fb3c07b2dbcbf28773b0138e85d";
        }

        public RegionResponse GetWeatherByRegion(string region)
        {
            RegionResponse response;

            var result = _httpClient.GetAsync($"{_host}/forecast/region/{region}?{_token}").GetAwaiter().GetResult();

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
            HourResponse errorResponse;

            if (parameterRequest == null || parameterRequest.State == null || parameterRequest.City == null)
            {
                errorResponse = new HourResponse() { Detail = "Parâmetros Inválidos" };
                return errorResponse;
            }

            var result = _httpClient.GetAsync($"{_host}/locale/city?name={parameterRequest.City}&state={parameterRequest.State}&{_token}")
                .Result;

            if (result.Content == null)
            {
                errorResponse = new HourResponse() { Detail = "Cidade ou Estado inválidos." };
                return errorResponse;
            }

            try
            {
                var response = result.Content.ReadAsAsync<HourResponse>().Result;
                

            }
            catch (UnsupportedMediaTypeException ex)
            {
                errorResponse = new HourResponse()
                {
                    Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente."
                };
                return errorResponse;
            }
            catch (Exception ex)
            {
                errorResponse = new HourResponse() { Detail = "" };
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

