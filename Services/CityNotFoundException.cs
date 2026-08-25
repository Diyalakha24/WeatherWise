namespace WeatherWise.Services
{
    public class CityNotFoundException : Exception
    {
        public CityNotFoundException(string city)
            : base($"City '{city}' was not found.")
        {
        }
    }
}