using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using WeatherAPI.Models.Region;

namespace ClimaTempoAPI.Interfaces
{
    public interface IWeatherService
    {
        RegionResponse GetWeatherByRegion(string region);
        CityResponse GetCurrentWeatherByCity(ParameterRequest city);

    }
}
