using Microsoft.AspNetCore.Mvc;
using WeatherWise.Services;

namespace WeatherWise.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        // GET: / or /Weather
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Weather/Search?city=Durban
        // Called via JavaScript fetch() from the page. Returns JSON.
        [HttpGet]
        public async Task<IActionResult> Search(string city)
        {
            var result = await _weatherService.GetWeatherAsync(city);
            return Json(result);
        }
    }
}