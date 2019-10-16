using ClimaTempoAPI.Models;
using ClimaTempoAPI.Models.Current;
using ClimaTempoAPI.Models.Days;
using ClimaTempoAPI.Models.Hour;
using WeatherAPI.Models.Region;

namespace ClimaTempoAPI.Interfaces
{
    public interface IWeatherService
    {
        RegionResponse GetWeatherByRegion(string region);
        CityResponse GetCurrentWeatherByCity(ParameterRequest city);
        HourResponse Get72hrWeatherById(ParameterRequest city);
        DaysResponse Get15DaysWeather(ParameterRequest city);
    }
}
