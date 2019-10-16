using ClimaTempoAPI.Interfaces;
using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using ClimaTempoAPI.Models.Days;
using ClimaTempoAPI.Models.Hour;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WeatherAPI.Models;
using WeatherAPI.Models.Region;
using ClimaTempoAPI.Service;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        IWeatherService _service;

        public WeatherController(IWeatherService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get current weather by region.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        [HttpGet("region/{region}")]
        [ProducesResponseType(typeof(RegionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult GetWeatherByRegion(string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                var result = new ErrorResponse();
                result.Message = "Região Inválida";
                return new BadRequestObjectResult(result);
            }
            try
            {
                var regionResponse = _service.GetWeatherByRegion(region);

                if (regionResponse == null)
                {
                    return new StatusCodeResult(StatusCodes.Status502BadGateway);
                }

                return new OkObjectResult(regionResponse);
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get current weather by city and state
        /// </summary>
        /// <param name="request"></param>        
        /// <returns></returns>
        [HttpGet("city/{city}/{state}")]
        [ProducesResponseType(typeof(CityClimate), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult GetCurrentWeatherByCity(ParameterRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.City) || string.IsNullOrEmpty(request.State))
            {
                var result = new ErrorResponse();
                result.Message = "Cidade ou Estado Inválidos";
                return new BadRequestObjectResult(result);
            }

            try
            {
                var result = _service.GetCurrentWeatherByCity(request);

                if (result == null)
                {
                    var errorResponse = new ErrorResponse();
                    errorResponse.Message = "Cidade invalida, verifique.";
                    return new BadRequestObjectResult(errorResponse);
                }

                return new OkObjectResult(result);

            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get the weather for the next 72 hours
        /// </summary>
        /// <param name="request"></param>      
        /// <returns></returns>
        [HttpGet("72hr/{city}/{state}")]
        [ProducesResponseType(typeof(HourResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult Get72hrWeatherById(ParameterRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.City) || string.IsNullOrEmpty(request.State))
            {
                var result = new ErrorResponse();
                result.Message = "Cidade ou Estado Inválidos";
                return new BadRequestObjectResult(result);
            }

            try
            {
                var result = _service.Get72hrWeatherById(request);

                if (result == null)
                {
                    var errorResponse = new ErrorResponse();
                    errorResponse.Message = "Cidade invalida, verifique.";
                    return new BadRequestObjectResult(errorResponse);
                }

                return new OkObjectResult(result);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get forecast for next 15 days.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("15days/{city}/{state}")]
        [ProducesResponseType(typeof(DaysResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult Get15daysWeatherById(ParameterRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.City) || string.IsNullOrEmpty(request.State))
            {
                var result = new ErrorResponse();
                result.Message = "Cidade ou Estado Inválidos";
                return new BadRequestObjectResult(result);
            }

            try
            {
                var result = _service.Get15DaysWeather(request);

                if (result == null)
                {
                    var errorResponse = new ErrorResponse();
                    errorResponse.Message = "Cidade invalida, verifique.";
                    return new BadRequestObjectResult(errorResponse);
                }

                return new OkObjectResult(result);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


    }
}