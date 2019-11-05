using ClimaTempoAPI.Interfaces;
using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using ClimaTempoAPI.Models.Days;
using ClimaTempoAPI.Models.Hour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            _token = "token=f444ae97bad0cadc04e972d4566220f1";
        }

        public RegionResponse GetWeatherByRegion(string region)
        {
            RegionResponse response;

            var result = _httpClient.GetAsync($"{_host}/forecast/region/{region}?{_token}").GetAwaiter().GetResult();

            if (result == null) return null;

            if (!result.IsSuccessStatusCode)
            {
                if (result.StatusCode == HttpStatusCode.BadRequest)
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
                    StatusCode = HttpStatusCode.BadRequest
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
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var baseModel = result.Content.ReadAsAsync<IEnumerable<BaseListModel>>().Result;

                if (baseModel == null || !baseModel.Any())
                {
                    return new CityResponse()
                    {
                        Detail = "Cidade ou Estado inválidos.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var cityID = baseModel.First().Id;
                var request = new HttpRequestMessage();
                request.Content = new StringContent($"localeId[]={cityID}");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                result = _httpClient.PutAsync("http://apiadvisor.climatempo.com.br/api-manager/user-token/f444ae97bad0cadc04e972d4566220f1/locales", request.Content)?.Result;

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return new CityResponse()
                    {
                        Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente.",

                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var resultIdSearch = _httpClient.GetAsync($"{_host}/weather/locale/{cityID}/current?{_token}")?.Result;
                var finalResult = resultIdSearch.Content.ReadAsAsync<CityResponse>().Result;
                finalResult.StatusCode = resultIdSearch.StatusCode;

                return finalResult;
            }
            catch (UnsupportedMediaTypeException)
            {
                return new CityResponse()
                {
                    Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente.",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception)
            {
                return new CityResponse()
                {
                    StatusCode = HttpStatusCode.BadGateway,
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
                    StatusCode = HttpStatusCode.BadRequest
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
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return errorResponse;
                }

                var baseModel = result.Content.ReadAsAsync<IEnumerable<BaseListModel>>().Result;

                if (baseModel == null || !baseModel.Any())
                {
                    return new HourResponse()
                    {
                        Detail = "Cidade ou Estado inválidos.",
                        Data = null,
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var cityID = baseModel.First().Id;
                var request = new HttpRequestMessage();
                request.Content = new StringContent($"localeId[]={cityID}");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                result = _httpClient.PutAsync("http://apiadvisor.climatempo.com.br/api-manager/user-token/f444ae97bad0cadc04e972d4566220f1/locales", request.Content)?.Result;

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return new HourResponse()
                    {
                        Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente.",
                        Data = null,
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                result = _httpClient.GetAsync($"{_host}/forecast/locale/{cityID}/hours/72?{_token}").Result;

                var finalResult = result.Content.ReadAsAsync<HourResponse>().Result;
                finalResult.StatusCode = result.StatusCode;

                return finalResult;
            }
            catch (UnsupportedMediaTypeException)
            {
                errorResponse = new HourResponse()
                {
                    Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente.",
                    Data = null,
                    StatusCode = HttpStatusCode.BadRequest
                };

                return errorResponse;
            }
            catch (Exception)
            {
                errorResponse = new HourResponse() { Detail = "Falha de comunicação.", Data = null, StatusCode = HttpStatusCode.BadGateway };
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
                    StatusCode = HttpStatusCode.BadRequest
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
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var baseModel = result.Content.ReadAsAsync<IEnumerable<BaseListModel>>().Result;

                if (baseModel == null || !baseModel.Any())
                {
                    return new DaysResponse()
                    {
                        Detail = "Cidade ou Estado inválidos.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var cityID = baseModel.First().Id;
                var request = new HttpRequestMessage();
                request.Content = new StringContent($"localeId[]={cityID}");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                result = _httpClient.PutAsync("http://apiadvisor.climatempo.com.br/api-manager/user-token/f444ae97bad0cadc04e972d4566220f1/locales", request.Content)?.Result;
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return new DaysResponse()
                    {
                        Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                result = _httpClient.GetAsync($"{_host}/forecast/locale/{cityID}/days/15?{_token}").Result;
                var finalResult = result.Content.ReadAsAsync<DaysResponse>().Result;
                finalResult.StatusCode = result.StatusCode;

                return finalResult;

            }
            catch (UnsupportedMediaTypeException)
            {
                return new DaysResponse()
                {
                    Detail = "Não foi possível executar a operação, verifique os " +
                    "Parametros informados e tente novamente.",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception)
            {
                return new DaysResponse() { Detail = "Falha de comunicação.", StatusCode = HttpStatusCode.BadGateway };
            }
        }
    }
}