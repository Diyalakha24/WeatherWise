using System.Text.Json.Serialization;

namespace WeatherWise.Models
{
    // Matches the response from the "current weather" endpoint
    public class OpenWeatherCurrentResponse
    {
        [JsonPropertyName("weather")]
        public List<WeatherCondition> Weather { get; set; } = new();

        [JsonPropertyName("main")]
        public MainInfo Main { get; set; } = new();

        [JsonPropertyName("wind")]
        public WindInfo Wind { get; set; } = new();

        [JsonPropertyName("sys")]
        public SysInfo Sys { get; set; } = new();

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class WeatherCondition
    {
        [JsonPropertyName("main")]
        public string Main { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }

    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }
    }

    public class WindInfo
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
    }

    public class SysInfo
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }
    }

    // Matches the response from the "5 day / 3 hour forecast" endpoint
    public class OpenWeatherForecastResponse
    {
        [JsonPropertyName("list")]
        public List<ForecastItem> List { get; set; } = new();
    }

    public class ForecastItem
    {
        [JsonPropertyName("dt_txt")]
        public string DtTxt { get; set; } = string.Empty;

        [JsonPropertyName("main")]
        public MainInfo Main { get; set; } = new();

        [JsonPropertyName("weather")]
        public List<WeatherCondition> Weather { get; set; } = new();
    }
}