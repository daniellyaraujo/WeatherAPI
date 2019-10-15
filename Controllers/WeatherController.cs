using ClimaTempoAPI.Interfaces;
using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using ClimaTempoAPI.Models.Days;
using ClimaTempoAPI.Models.Hour;
using ClimaTempoAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WeatherAPI.Models;
using WeatherAPI.Models.Region;

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
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }            
        }

        /// <summary>
        /// Get current weather by city and state
        /// </summary>
        /// <param name="city"></param>        
        /// <returns></returns>
        [HttpGet("city/{city}/{state}")]
        [ProducesResponseType(typeof(CityClimate), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult GetCurrentWeatherByCity(ParameterRequest city)
        {
            
            if (string.IsNullOrEmpty(city.City))
            {
                var result = new ErrorResponse();
                result.Message = "Cidade ou Estado Inválidos";
                return new BadRequestObjectResult(result);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the weather for the next 72 hours
        /// </summary>
        /// <param name="city"></param>      
        /// <returns></returns>
        [HttpGet("72hr/{city}/{state}")]
        [ProducesResponseType(typeof(HourResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult Get72hrWeatherById(ParameterRequest city)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get forecast for next 15 days.
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet("15days/{city}/{state}")]
        [ProducesResponseType(typeof(DaysResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult Get15daysWeather(ParameterRequest city)
        {
            throw new NotImplementedException();
        }


    }
}