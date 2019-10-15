using ClimaTempoAPI.Interfaces;
using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Models.Region;

namespace ClimaTempoAPI.Service
{
    public class WeatherService : IWeatherService
    {
        public RegionResponse GetWeatherByRegion(string region)
        {
            throw new NotImplementedException();
        }
        public CityResponse GetCurrentWeatherByCity(ParameterRequest city)
        {
            throw new NotImplementedException();
        }
    }
}
