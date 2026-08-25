namespace WeatherWise.Models
{
    public class WeatherViewModel
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int Pressure { get; set; }
        public string Sunrise { get; set; } = string.Empty;
        public string Sunset { get; set; } = string.Empty;

        public List<ForecastDayViewModel> Forecast { get; set; } = new();
    }

    public class ForecastDayViewModel
    {
        public string Day { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }
}