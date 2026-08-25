using System.Net;
using System.Text.Json;
using WeatherWise.Models;

namespace WeatherWise.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration configuration, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = configuration["OpenWeatherMap:ApiKey"] ?? string.Empty;
        }

        public async Task<WeatherViewModel> GetWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return Failure("Please enter a city name.");
            }

            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                _logger.LogError("OpenWeatherMap API key is missing from configuration.");
                return Failure("Unable to retrieve weather data. Please try again later.");
            }

            try
            {
                var weather = await GetCurrentWeatherAsync(city);
                weather.Forecast = await GetForecastAsync(city);
                weather.Success = true;
                return weather;
            }
            catch (CityNotFoundException)
            {
                return Failure("City not found. Please check the city name and try again.");
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Weather API request timed out for city {City}", city);
                return Failure("Unable to retrieve weather data. Please try again later.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error while retrieving weather for city {City}", city);
                return Failure("Unable to retrieve weather data. Please try again later.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse weather API response for city {City}", city);
                return Failure("Unable to retrieve weather data. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retrieving weather for city {City}", city);
                return Failure("Unable to retrieve weather data. Please try again later.");
            }
        }

        private async Task<WeatherViewModel> GetCurrentWeatherAsync(string city)
        {
            var url = $"weather?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric";
            using var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new CityNotFoundException(city);
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<OpenWeatherCurrentResponse>(json)
                ?? throw new JsonException("Empty current weather response.");

            var condition = data.Weather.FirstOrDefault();

            return new WeatherViewModel
            {
                City = data.Name,
                Country = data.Sys.Country,
                Temperature = Math.Round(data.Main.Temp, 1),
                FeelsLike = Math.Round(data.Main.FeelsLike, 1),
                Humidity = data.Main.Humidity,
                Pressure = data.Main.Pressure,
                WindSpeed = Math.Round(data.Wind.Speed * 3.6, 1), // m/s -> km/h
                Condition = CapitaliseFirstLetter(condition?.Description),
                IconUrl = BuildIconUrl(condition?.Icon),
                Sunrise = UnixToLocalTime(data.Sys.Sunrise),
                Sunset = UnixToLocalTime(data.Sys.Sunset)
            };
        }

        private async Task<List<ForecastDayViewModel>> GetForecastAsync(string city)
        {
            var url = $"forecast?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric";
            using var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new CityNotFoundException(city);
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<OpenWeatherForecastResponse>(json)
                ?? throw new JsonException("Empty forecast response.");

            // The API returns a reading every 3 hours. Taking the midday (12:00) reading
            // gives one simple, representative entry per day for the next 5 days.
            var middayReadings = data.List
                .Where(item => item.DtTxt.Contains("12:00:00"))
                .Take(5);

            var forecast = new List<ForecastDayViewModel>();

            foreach (var item in middayReadings)
            {
                var date = DateTime.Parse(item.DtTxt);
                var condition = item.Weather.FirstOrDefault();

                forecast.Add(new ForecastDayViewModel
                {
                    Day = date.ToString("ddd"),
                    Temperature = Math.Round(item.Main.Temp, 1),
                    Condition = condition?.Main ?? "Unknown",
                    IconUrl = BuildIconUrl(condition?.Icon)
                });
            }

            return forecast;
        }

        private static WeatherViewModel Failure(string message) =>
            new() { Success = false, ErrorMessage = message };

        private static string BuildIconUrl(string? iconCode) =>
            string.IsNullOrEmpty(iconCode) ? string.Empty : $"https://openweathermap.org/img/wn/{iconCode}@2x.png";

        private static string UnixToLocalTime(long unixSeconds) =>
            unixSeconds == 0 ? "--" : DateTimeOffset.FromUnixTimeSeconds(unixSeconds).ToLocalTime().ToString("HH:mm");

        private static string CapitaliseFirstLetter(string? text) =>
            string.IsNullOrEmpty(text) ? "Unknown" : char.ToUpper(text[0]) + text[1..];
    }
}