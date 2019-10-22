using ClimaTempoAPI.Interfaces;
using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using ClimaTempoAPI.Models.Days;
using ClimaTempoAPI.Models.Hour;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public CityResponse GetCurrentWeatherByCity(ParameterRequest parameterRequest)
        {
            CityResponse response;

            if (parameterRequest == null || parameterRequest.State == null || parameterRequest.City == null)
            {
                response = new CityResponse()
                {
                    Detail = "Parâmetros Inválidos",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return response;
            }

            try
            {
                var result = _httpClient.GetAsync($"{_host}/locale/city?name={parameterRequest.City}&state={parameterRequest.State}&{_token}").Result;

                if (result.Content == null)
                {
                    return new CityResponse()
                    {
                        Detail = "Cidade ou Estado inválidos.",
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                var baseModel = result.Content.ReadAsAsync<IEnumerable<BaseListModel>>().Result;

                if (baseModel == null || !baseModel.Any())
                {
                    return new CityResponse()
                    {
                        Detail = "Cidade ou Estado inválidos.",
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                var cityID = baseModel.First().Id;

                //inserçao do put para gravar ID para pesquisa
               // result = _httpClient.PutAsync("http://apiadvisor.climatempo.com.br/api-manager/user-token/0b5e7fb3c07b2dbcbf28773b0138e85d/locales", )?.Result;

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new CityResponse()
                    {
                        Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente.",
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                result = _httpClient.GetAsync($"{_host}/weather/locale/{baseModel.First().Id}/current?{_token}")?.Result;

                var finalResult = result.Content.ReadAsAsync<CityResponse>().Result;
                finalResult.StatusCode = result.StatusCode;

                return finalResult;
            }
            catch (UnsupportedMediaTypeException ex)
            {
                return new CityResponse()
                {
                    Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente.",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new CityResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.BadGateway,
                    Detail = "Falha de comunicação."
                };
            }
        }

        public HourResponse Get72hrWeatherById(ParameterRequest parameterRequest)
        {
            HourResponse errorResponse;

            if (parameterRequest == null || parameterRequest.State == null || parameterRequest.City == null)
            {
                errorResponse = new HourResponse()
                {
                    Detail = "Parâmetros Inválidos",
                    Data = null,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return errorResponse;
            }

            try
            {
                var result = _httpClient.GetAsync($"{_host}/locale/city?name={parameterRequest.City}&state={parameterRequest.State}&{_token}").Result;
                if (result.Content == null)
                {
                    errorResponse = new HourResponse()
                    {
                        Detail = "Cidade ou Estado inválidos.",
                        Data = null,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                    return errorResponse;
                }
                var response = result.Content.ReadAsAsync<BaseListModel>().Result;

                result = _httpClient.GetAsync($"{_host}forecast/locale/{response.Id}/hours/72?{_token}").Result;

                var finalResult = result.Content.ReadAsAsync<HourResponse>().Result;
                finalResult.StatusCode = result.StatusCode;

                return finalResult;
            }
            catch (UnsupportedMediaTypeException ex)
            {
                errorResponse = new HourResponse()
                {
                    Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente.",
                    Data = null,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return errorResponse;

            }
            catch (Exception ex)
            {
                errorResponse = new HourResponse() { Detail = "Falha de comunicação.", Data = null, StatusCode = System.Net.HttpStatusCode.BadGateway };
                return errorResponse;
            }
        }

        public DaysResponse Get15DaysWeather(ParameterRequest parameterRequest)
        {
            DaysResponse response;

            if (parameterRequest == null || parameterRequest.State == null || parameterRequest.City == null)
            {
                response = new DaysResponse()
                {
                    Detail = "Parâmetros Inválidos",
                    Date = null,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return response;
            }

            try
            {
                var result = _httpClient.GetAsync($"{_host}/locale/city?name={parameterRequest.City}&state={parameterRequest.State}&{_token}").Result;

                if (result.Content == null)
                {
                    return new DaysResponse()
                    {
                        Detail = "Cidade ou Estado inválidos.",
                        Date = null,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                var baseModel = result.Content.ReadAsAsync<BaseListModel>().Result;

                result = _httpClient.GetAsync($"{_host}forecast/locale/{baseModel.Id}/days/15?{_token}").Result;

                var finalResult = result.Content.ReadAsAsync<DaysResponse>().Result;
                finalResult.StatusCode = result.StatusCode;

                return finalResult;
            }
            catch (UnsupportedMediaTypeException ex)
            {
                return new DaysResponse()
                {
                    Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente.",
                    Date = null,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new DaysResponse() { Detail = "Falha de comunicação.", Date = null, StatusCode = System.Net.HttpStatusCode.BadGateway };

            }
        }
    }
}

