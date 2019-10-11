using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using ClimaTempoAPI.Models.Days;
using ClimaTempoAPI.Models.Hour;
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
        /// <summary>
        /// Get current weather by region.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>

        [HttpGet("region/{region}")]
        [ProducesResponseType(typeof(RegionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult GetWeatherByRegion(string region)
        {


            throw new NotImplementedException();
        }

        /// <summary>
        /// Get current weather by city and state
        /// </summary>
        /// <param name="city"></param>        
        /// <returns></returns>

        [HttpGet("city/{city}/{state}")]
        [ProducesResponseType(typeof(CityClimate), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult GetCurrentWeatherByCity(ParameterRequest city)
        {        
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the weather for the next 72 hours
        /// </summary>
        /// <param name="city"></param>      
        /// <returns></returns>
        [HttpGet("72hr/{city}/{state}")]
        [ProducesResponseType(typeof(HourResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult Get15daysWeather(ParameterRequest city)
        {
            throw new NotImplementedException();
        }


    }
}