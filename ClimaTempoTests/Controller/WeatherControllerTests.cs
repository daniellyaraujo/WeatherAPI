using ClimaTempoAPI.Interfaces;
using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using ClimaTempoAPI.Models.Days;
using ClimaTempoAPI.Models.Hour;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Net;
using WeatherAPI.Controllers;
using WeatherAPI.Models;
using WeatherAPI.Models.Region;
using Xunit;

namespace ClimaTempoTests.Controller
{
    public class WeatherControllerTests
    {
        IWeatherService _service;
        public WeatherControllerTests()
        {
            _service = Substitute.For<IWeatherService>();
        }

        #region GetWeatherByRegion tests
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetWeatherByRegion_When_Parameter_IsNull_OrEmpty_Returns_BadRequest(string region)
        {
            var errorResponse = new ErrorResponse();
            var controller = new WeatherController(_service);

            errorResponse.Message = "Região Inválida";
            var response = controller.GetWeatherByRegion(region);

            response.Should().BeEquivalentTo(new BadRequestObjectResult(errorResponse));
        }

        [Fact]
        public void GetWeatherByRegion_When_CustomerService_Throws_Exceptions_Returns_InternalServerError()
        {
            var region = "sul";
            _service.GetWeatherByRegion(region).Throws(new Exception());
            var controller = new WeatherController(_service);
            var result = controller.GetWeatherByRegion(region);
            result.Should().BeEquivalentTo(new StatusCodeResult(StatusCodes.Status500InternalServerError));
        }

        [Fact]
        public void GetWeatherByRegion_When_CustomerService_Returns_Null_Returns_BadRequest()
        {
            var region = "regiaoinvalida";
            RegionResponse response = null;
            var controller = new WeatherController(_service);
            var errorResponse = new ErrorResponse() { Message = "Região Inválida" };

            _service.GetWeatherByRegion(region).Returns(response);

            var result = controller.GetWeatherByRegion(region);
            result.Should().BeEquivalentTo(new StatusCodeResult(StatusCodes.Status400BadRequest));
        }

        [Fact]
        public void GetWeatherByRegion_When_Has_OK_Returns_Success()
        {
            var controller = new WeatherController(_service);
            var response = new RegionResponse();
            var request = new Region();
            var region = "sudeste";

            request.Date = new DateTime(2019, 10, 13);
            request.Text = "Hoje o dia está nublado";

            response.Data.Add(request);

            _service.GetWeatherByRegion(region).Returns(response);

            var result = controller.GetWeatherByRegion(region);

            result.Should().BeEquivalentTo(new OkObjectResult(response));
        }

        #endregion

        #region GetCurrentWeatherByCity tests
        [Theory]
        [InlineData(null, null)]
        [InlineData("Mauá", "")]
        [InlineData("Mauá", null)]
        [InlineData("", "SP")]
        [InlineData(null, "SP")]
        public void GetCurrentWeatherByCity_When_Parameters_IsNull_OrEmpty_Returns_BadRequest(string city, string state)
        {
            ParameterRequest request = null;

            if (city != null || state != null)
                request = new ParameterRequest { City = city, State = state };

            var result = new ErrorResponse();
            result.Message = "Cidade ou Estado Inválidos";
            var controller = new WeatherController(_service);
            var response = controller.GetCurrentWeatherByCity(request);

            response.Should().BeEquivalentTo(new BadRequestObjectResult(result));
        }

        [Fact]
        public void GetCurrentWeatherByCity_When_CustomerService_Throws_Exceptions_Returns_InternalServerError()
        {
            var parameterRequest = new ParameterRequest();

            parameterRequest.City = "Mauá";
            parameterRequest.State = "SP";

            var controller = new WeatherController(_service);
            _service.GetCurrentWeatherByCity(parameterRequest).Throws(new Exception());

            var result = controller.GetCurrentWeatherByCity(parameterRequest);
            result.Should().BeEquivalentTo(new StatusCodeResult(StatusCodes.Status500InternalServerError));
        }

        [Fact]
        public void GetCurrentWeatherByCity_When_Has_OK_Returns_Success()
        {
            var request = new ParameterRequest();

            request.City = "Mauá";
            request.State = "SP";

            var response = new CityResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Detail = "Falha de comunicação.",
                Id = 3954,
                Name = "Nublado",
                State = "SP",
                Country = "Brasil",
            };

            _service.GetCurrentWeatherByCity(request).Returns(response);

            var controller = new WeatherController(_service);

            var result = controller.GetCurrentWeatherByCity(request);
            result.Should().BeEquivalentTo(new OkObjectResult(response));
        }

        [Fact]
        public void GetCurrentWeatherByCity_When_InvalidCity_Returns_BadRequest()
        {
            var request = new ParameterRequest() { City = "Mauaaa", State = "SP" };
            var errorMessage = new ErrorResponse() { Message = "Cidade invalida, verifique." };

            CityResponse response = null;

            _service.GetCurrentWeatherByCity(request).Returns(response);

            var controller = new WeatherController(_service);

            var result = controller.GetCurrentWeatherByCity(request);

            result.Should().BeEquivalentTo(new BadRequestObjectResult(errorMessage));
        }
        #endregion

        #region Get72HrWeatherById tests
        [Theory]
        [InlineData(null, null)]
        [InlineData("Mauá", "")]
        [InlineData("Mauá", null)]
        [InlineData("", "SP")]
        [InlineData(null, "SP")]
        public void Get72hrWeatherById_When_Parameter_IsNull_OrEmpty_Returns_BadRequest(string city, string state)
        {
            var request = new ParameterRequest() { City = null, State = null };

            if (city == null && state == null)
                request = null;
            else
            {
                request.City = city;
                request.State = state;
            }

            var result = new ErrorResponse();
            result.Message = "Cidade ou Estado Inválidos";
            var controller = new WeatherController(_service);
            var response = controller.Get72hrWeatherById(request);

            response.Should().BeEquivalentTo(new BadRequestObjectResult(result));
        }

        [Fact]
        public void Get72WeatherById_When_CustomerService_Throws_Exceptions_Returns_InternalServerError()
        {
            var parameterRequest = new ParameterRequest() { City = "Mauá", State = "SP" };

            var controller = new WeatherController(_service);
            _service.Get72hrWeatherById(parameterRequest).Throws(new System.Exception());

            var result = controller.Get72hrWeatherById(parameterRequest);
            result.Should().BeEquivalentTo(new StatusCodeResult(StatusCodes.Status500InternalServerError));
        }

        [Fact]
        public void Get72WeatherById_When_Has_OK_Returns_Success()
        {
            var request = new ParameterRequest() { City = "Mauá", State = "SP" };
            var controller = new WeatherController(_service);
            var response = Substitute.For<HourResponse>();
            _service.Get72hrWeatherById(request).Returns(response);
            var result = controller.Get72hrWeatherById(request);
            result.Should().BeEquivalentTo(new OkObjectResult(response));
        }

        [Fact]
        public void Get72WeatherById_When_InvalidCity_Returns_BadRequest()
        {
            var request = new ParameterRequest() { City = "Mauaaa", State = "SP" };
            var errorMessage = new ErrorResponse() { Message = "Cidade invalida, verifique." };

            HourResponse response = null;

            _service.Get72hrWeatherById(request).Returns(response);

            var controller = new WeatherController(_service);

            var result = controller.Get72hrWeatherById(request);

            result.Should().BeEquivalentTo(new BadRequestObjectResult(errorMessage));
        }

        #endregion

        #region Get15DaysWeatherById
        [Theory]
        [InlineData(null, null)]
        [InlineData("Mauá", "")]
        [InlineData("Mauá", null)]
        [InlineData("", "SP")]
        [InlineData(null, "SP")]
        public void Get15DaysWeatherById_When_Parameter_IsNull_OrEmpty_Returns_BadRequest(string city, string state)
        {
            var request = new ParameterRequest() { City = null, State = null };

            if (city == null && state == null)
                request = null;
            else
            {
                request.City = city;
                request.State = state;
            }

            var result = new ErrorResponse();
            result.Message = "Cidade ou Estado Inválidos";
            var controller = new WeatherController(_service);
            var response = controller.Get15daysWeatherById(request);

            response.Should().BeEquivalentTo(new BadRequestObjectResult(result));
        }

        [Fact]
        public void Get15DaysWeatherById_When_CustomerService_Throws_Exceptions_Returns_InternalServerError()
        {
            var parameterRequest = new ParameterRequest() { City = "Mauá", State = "SP" };
            var controller = new WeatherController(_service);
            _service.Get15DaysWeather(parameterRequest).Throws(new Exception());
            var result = controller.Get15daysWeatherById(parameterRequest);
            result.Should().BeEquivalentTo(new StatusCodeResult(StatusCodes.Status500InternalServerError));
        }

        [Fact]
        public void Get15DaysWeatherById_When_Has_OK_Returns_Success()
        {
            var request = new ParameterRequest() { City = "Mauá", State = "SP" };
            var controller = new WeatherController(_service);
            var response = Substitute.For<DaysResponse>();
            _service.Get15DaysWeather(request).Returns(response);
            var result = controller.Get15daysWeatherById(request);
            result.Should().BeEquivalentTo(new OkObjectResult(response));
        }

        [Fact]
        public void Get15DaysWeatherById_When_InvalidCity_Returns_BadRequest()
        {
            var request = new ParameterRequest() { City = "Mauaaa", State = "SP" };
            var errorMessage = new ErrorResponse() { Message = "Cidade invalida, verifique." };

            DaysResponse response = null;

            _service.Get15DaysWeather(request).Returns(response);

            var controller = new WeatherController(_service);

            var result = controller.Get15daysWeatherById(request);

            result.Should().BeEquivalentTo(new BadRequestObjectResult(errorMessage));
        }

        #endregion
    }
}