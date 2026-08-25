using WeatherWise.Models;

namespace WeatherWise.Services
{
    public interface IWeatherService
    {
        Task<WeatherViewModel> GetWeatherAsync(string city);
    }
}