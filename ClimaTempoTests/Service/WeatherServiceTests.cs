using AutoFixture;
using ClimaTempoAPI.Interfaces;
using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using ClimaTempoAPI.Models.Days;
using ClimaTempoAPI.Models.Hour;
using ClimaTempoAPI.Service;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WeatherAPI.Models.Region;
using Xunit;

namespace ClimaTempoTests.Service
{
    public class WeatherServiceTests
    {
        #region Fields

        private IWeatherService _service;
        private IHttpClient _client;
        private Fixture _fixture;
        public string _host;
        public string _token;
        private IConfiguration _configuration;
        private string _hostPut;

        #endregion

        #region Construtor
        public WeatherServiceTests()
        {
            _client = Substitute.For<IHttpClient>();
            _configuration = Substitute.For<IConfiguration>();

            _host = "http://apiadvisor.climatempo.com.br/api/v1";
            _token = "token=f444ae97bad0cadc04e972d4566220f1";
            _hostPut = "http://apiadvisor.climatempo.com.br/api-manager/user-token";

            _configuration.GetSection("ClimaTempo:host").Value.Returns(_host);
            _configuration.GetSection("ClimaTempo:token").Value.Returns(_token);
            _configuration.GetSection("ClimaTempo:hostPut").Value.Returns(_hostPut);

            _service = new WeatherService(_client, _configuration);
            _fixture = new Fixture();
        }

        #endregion

        #region Constructor Tests

        //[Fact]
        //public void WeatherService_When_Constructor_Receive_InvalidParameters_Returns_ArgumentNullException()
        //{
        //    var service = new WeatherService(null, null);

        //    service.Should().BeNull();
        //}

        #endregion

        #region GetWeatherByRegion Tests

        [Fact]
        public void GetWeatherByRegionTests_When_IsNull_Returns_Null()
        {
            _client.GetAsync($"{_host}/forecast/region/sul?{_token}").Returns((HttpResponseMessage)null);
            var result = _service.GetWeatherByRegion("sul");
            result.Should().BeNull();
        }

        [Fact]
        public void GetWeatherByRegionTests_When_IsBadRequest_Returns_Detail_And_BadRequestStatusCode()
        {
            var region = "sul";

            var response = _fixture.Build<RegionResponse>()
                                   .With(x => x.Detail, "Region must be one by Norte,Nordeste,Centro-Oeste,Sul,Sudeste - stul given")
                                   .With(x => x.StatusCode, HttpStatusCode.BadRequest)
                                   .With(x => x.Data, (IList<Region>)null)
                                   .Create();

            var jsonFile = JsonConvert.SerializeObject(response);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            httpResponseMessage.Content = new StringContent(jsonFile);
            httpResponseMessage.Content.Headers.ContentType.MediaType = "application/json";

            _client.GetAsync($"{_host}/forecast/region/{region}?{_token}").Returns(httpResponseMessage);
            var result = _service.GetWeatherByRegion(region);
            result.Should().BeEquivalentTo(response);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public void GetWeatherByRegionTests_When_IsNotSuccess_Returns_StatusCode(HttpStatusCode statusCode)
        {
            var region = "sul";

            var httpResponseMessage = new HttpResponseMessage(statusCode);

            _client.GetAsync($"{_host}/forecast/region/{region }?{_token}")
                .Returns(httpResponseMessage);
            var response = new RegionResponse { StatusCode = statusCode };

            var result = _service.GetWeatherByRegion(region);
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void GetWeatherByRegionTests_When_Success_Returns_RegionResponse()
        {
            var region = "sul";

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            var response = _fixture.Build<RegionResponse>()
                         .With(x => x.StatusCode, HttpStatusCode.OK).Create();

            var jsonfile = JsonConvert.SerializeObject(response);
            httpResponseMessage.Content = new StringContent(jsonfile);
            httpResponseMessage.Content.Headers.ContentType.MediaType = "application/json";

            _client.GetAsync($"{_host}/forecast/region/{region}?{_token}").ReturnsForAnyArgs(httpResponseMessage);

            var result = _service.GetWeatherByRegion(region);
            result.Should().BeEquivalentTo(response);
        }

        #endregion

        #region Get72hrWeatherById Tests

        [Theory]
        [InlineData(null, null, false)]
        [InlineData("Mauá", null, true)]
        [InlineData(null, null, true)]
        public void Get72hrWeatherById_When_Parameter_IsNull_OrEmpty_Returns_BadRequest(string city, string state, bool hasInstance)
        {
            var request = !hasInstance ? null : new ParameterRequest() { City = city, State = state };

            var result = _service.Get72hrWeatherById(request);
            var errorResponse = new HourResponse()
            {
                Detail = "Parâmetros Inválidos",
                Data = null,
                StatusCode = HttpStatusCode.BadRequest
            };
            result.Should().BeEquivalentTo(errorResponse);
        }

        [Theory]
        [InlineData(false, "Cidade ou Estado inválidos.")]
        [InlineData(true, "Não foi possível executar a operação, verifique os Parametros informados e tente novamente.")]
        public void Get72hrWeatherById_When_Parameter_IsIncorrect_Returns_BadRequest(bool hasValue, string errorMessage)
        {
            var request = new ParameterRequest() { City = "maue", State = "SP" };

            var httpResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };

            httpResponse.Content = !hasValue ? null : new StringContent("vazio");

            _client.GetAsync($"{_host}/locale/city?name={request.City}&state={request.State}&{_token}").Returns(httpResponse);

            var result = _service.Get72hrWeatherById(request);

            var errorResponse = new HourResponse();
            errorResponse.Detail = errorMessage;
            errorResponse.StatusCode = HttpStatusCode.BadRequest;
            errorResponse.Data = null;

            result.Should().BeEquivalentTo(errorResponse);
        }

        [Fact]
        public void Get72hrWeatherById_When_Success_Returns_HourResponse()
        {
            var request = new ParameterRequest() { City = "Mauá", State = "SP" };
            var baseM = _fixture.Build<BaseListModel>().With(x => x.Country, "BR")
                .With(x => x.Id, 1234)
                .With(x => x.Name, "Mauá")
                .With(x => x.State, "SP").CreateMany(2);

            var cityID = 1234;
            var response = _fixture.Create<HourResponse>();
            response.StatusCode = HttpStatusCode.OK;

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            var jsonfile = JsonConvert.SerializeObject(baseM);
            httpResponseMessage.Content = new StringContent(jsonfile);
            httpResponseMessage.Content.Headers.ContentType.MediaType = "application/json";

            _client.GetAsync($"{_host}/locale/city?name={request.City}&state={request.State}&{_token}").Returns(httpResponseMessage);


            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            var httpRequest = new HttpRequestMessage();
            httpRequest.Content = new StringContent($"localeId[]={cityID}");
            httpRequest.Content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";

            _client.PutAsync($"{_hostPut}/{_token}/locales", httpRequest.Content).ReturnsForAnyArgs(httpResponse);

            var httpResponseMessageResult = new HttpResponseMessage(HttpStatusCode.OK);

            jsonfile = JsonConvert.SerializeObject(response);
            httpResponseMessageResult.Content = new StringContent(jsonfile);
            httpResponseMessageResult.Content.Headers.ContentType.MediaType = "application/json";

            _client.GetAsync($"{_host}/forecast/locale/1234/hours/72?{_token}").Returns(httpResponseMessageResult);

            var result = _service.Get72hrWeatherById(request);

            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void Get72hrWeatherById_When_ThrowsException_Returns_ErrorResponse()
        {
            var request = new ParameterRequest() { City = "Mauá", State = "SP" };

            _client.GetAsync($"{_host}/locale/city?name={request.City}&state={request.State}&{_token}").Throws(new Exception());

            var result = _service.Get72hrWeatherById(request);

            var errorMessage = _fixture.Build<HourResponse>().With(x => x.StatusCode, HttpStatusCode.BadGateway)
                                                             .With(x => x.Data, (List<Hour>)null)
                                                             .With(x => x.Detail, "Falha de comunicação.").Create();

            result.Should().BeEquivalentTo(errorMessage);
        }
        #endregion

        #region Get15DaysWeather Tests
        [Theory]
        [InlineData(null, null, false)]
        [InlineData("Mauá", null, true)]
        [InlineData(null, null, true)]
        public void GetDaysWeather_When_Parameter_IsNull_OrEmpty_Returns_BadRequest(string city, string state, bool hasInstance)
        {
            var request = !hasInstance ? null : new ParameterRequest() { City = city, State = state };

            var result = _service.Get15DaysWeather(request);
            var response = new DaysResponse
            {
                Detail = "Parâmetros Inválidos",
                StatusCode = HttpStatusCode.BadRequest
            };
            result.Should().BeEquivalentTo(response);
        }

        [Theory]
        [InlineData(false, "Cidade ou Estado inválidos.")]
        [InlineData(true, "Não foi possível executar a operação, verifique os Parametros informados e tente novamente.")]
        public void GetDaysWeather_When_Parameter_IsIncorrect_Returns_BadRequest(bool hasValue, string errorMessage)
        {
            var request = new ParameterRequest() { City = "maue", State = "SP" };

            var httpResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };

            httpResponse.Content = !hasValue ? null : new StringContent("vazio");

            _client.GetAsync($"{_host}/locale/city?name={request.City}&state={request.State}&{_token}").Returns(httpResponse);

            var result = _service.Get15DaysWeather(request);

            var response = new DaysResponse();
            response.Detail = errorMessage;
            response.StatusCode = HttpStatusCode.BadRequest;

            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void GetDaysWeather_When_Success_Returns_CityResponse()
        {
            var request = new ParameterRequest() { City = "Mauá", State = "SP" };
            var baseM = _fixture.Build<BaseListModel>().With(x => x.Country, "BR")
                .With(x => x.Id, 1234)
                .With(x => x.Name, "Mauá")
                .With(x => x.State, "SP").CreateMany(2);

            var cityID = 1234;
            var response = _fixture.Create<DaysResponse>();
            response.StatusCode = HttpStatusCode.OK;

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var jsonfile = JsonConvert.SerializeObject(baseM);
            httpResponseMessage.Content = new StringContent(jsonfile);
            httpResponseMessage.Content.Headers.ContentType.MediaType = "application/json";

            _client.GetAsync($"{_host}/locale/city?name={request.City}&state={request.State}&{_token}").Returns(httpResponseMessage);

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var httpRequest = new HttpRequestMessage();
            httpRequest.Content = new StringContent($"localeId[]={cityID}");
            httpRequest.Content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
            _client.PutAsync("http://apiadvisor.climatempo.com.br/api-manager/user-token/f444ae97bad0cadc04e972d4566220f1/locales", httpRequest.Content).ReturnsForAnyArgs(httpResponse);

            var httpResponseMessageResult = new HttpResponseMessage(HttpStatusCode.OK);
            jsonfile = JsonConvert.SerializeObject(response);
            httpResponseMessageResult.Content = new StringContent(jsonfile);
            httpResponseMessageResult.Content.Headers.ContentType.MediaType = "application/json";

            _client.GetAsync($"{_host}/forecast/locale/1234/days/15?{_token}").Returns(httpResponseMessageResult);
            var result = _service.Get15DaysWeather(request);

            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void GetDaysWeather_When_ThrowsException_Returns_ErrorResponse()
        {
            var request = new ParameterRequest() { City = "Mauá", State = "SP" };

            _client.GetAsync($"{_host}/locale/city?name={request.City}&state={request.State}&{_token}").Throws(new Exception());

            var result = _service.Get15DaysWeather(request);

            var response = new DaysResponse() { Detail = "Falha de comunicação.", StatusCode = HttpStatusCode.BadGateway };

            result.Should().BeEquivalentTo(response);
        }

        #endregion

        #region GetCurrentWeatherByCity Tests

        [Theory]
        [InlineData(null, null, false)]
        [InlineData("Mauá", null, true)]
        [InlineData(null, null, true)]
        public void GetCurrentWeatherByCity_When_IsNull_Returns_Null(string city, string state, bool hasInstance)
        {
            var request = !hasInstance ? null : new ParameterRequest() { City = city, State = state };

            var result = _service.GetCurrentWeatherByCity(request);
            var response = new CityResponse()
            {
                Detail = "Parâmetros Inválidos",
                StatusCode = HttpStatusCode.BadRequest
            };
            result.Should().BeEquivalentTo(response);
        }


        [Theory]
        [InlineData(false, "Cidade ou Estado inválidos.")]
        [InlineData(true, "Não foi possível executar a operação, verifique os Parametros informados e tente novamente.")]
        public void GetCurrentWeatherByCity_When_Parameter_IsIncorrect_Returns_BadRequest(bool hasValue, string errorMessage)
        {
            var request = new ParameterRequest() { City = "maue", State = "SP" };

            var httpResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };

            httpResponse.Content = !hasValue ? null : new StringContent("vazio");

            _client.GetAsync($"{_host}/locale/city?name={request.City}&state={request.State}&{_token}").ReturnsForAnyArgs(httpResponse);

            var result = _service.GetCurrentWeatherByCity(request);

            var response = new CityResponse();
            response.Detail = errorMessage;
            response.StatusCode = HttpStatusCode.BadRequest;

            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void GetCurrentWeatherByCity_When_Success_Returns_CityResponse()
        {
            var request = new ParameterRequest() { City = "Mauá", State = "SP" };

            var baseM = _fixture.Build<BaseListModel>()
                                .With(x => x.Id, 1234)
                                .With(x => x.Name, "Mauá")
                                .With(x => x.State, "SP")
                                .With(x => x.Country, "BR")
                                .CreateMany(1);

            var response = new CityResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Detail = "Falha de comunicação.",
                Id = 1234,
                Name = "Nublado",
                State = "SP",
                Country = "Brasil",
            };

            var cityID = 1234;

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var jsonfile = JsonConvert.SerializeObject(baseM);
            httpResponseMessage.Content = new StringContent(jsonfile);
            httpResponseMessage.Content.Headers.ContentType.MediaType = "application/json";
            _client.GetAsync($"{_host}/locale/city?name={request.City}&state={request.State}&{_token}").Returns(httpResponseMessage);

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var httpRequest = new HttpRequestMessage();

            httpRequest.Content = new StringContent($"localeId[]={cityID}");
            httpRequest.Content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
            _client.PutAsync("http://apiadvisor.climatempo.com.br/api-manager/user-token/f444ae97bad0cadc04e972d4566220f1/locales", httpRequest.Content).ReturnsForAnyArgs(httpResponse);

            var httpResponseMessageResult = new HttpResponseMessage(HttpStatusCode.OK);
            jsonfile = JsonConvert.SerializeObject(response);
            httpResponseMessageResult.Content = new StringContent(jsonfile);
            httpResponseMessageResult.Content.Headers.ContentType.MediaType = "application/json";
            _client.GetAsync($"{_host}/weather/locale/1234/current?{_token}").Returns(httpResponseMessageResult);

            var result = _service.GetCurrentWeatherByCity(request);
            result.Should().BeEquivalentTo(response);
        }


        [Fact]
        public void GetCurrentWeatherByCity_When_ThrowsException_Returns_ErrorResponse()
        {
            var request = new ParameterRequest() { City = "Mauá", State = "SP" };

            _client.GetAsync($"{_host}/locale/city?name={request.City}&state={request.State}&{_token}").Throws(new Exception());

            var result = _service.GetCurrentWeatherByCity(request);

            var errorMessage = new CityResponse { StatusCode = HttpStatusCode.BadGateway, Detail = "Falha de comunicação." };

            result.Should().BeEquivalentTo(errorMessage);
        }
        #endregion
    }
}