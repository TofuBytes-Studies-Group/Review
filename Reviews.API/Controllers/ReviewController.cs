using Microsoft.AspNetCore.Mvc;
using Reviews.API.DTOs;
using Reviews.API.Services;
using Reviews.Domain.Entities;

namespace Reviews.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ReviewController> _logger;
        private readonly ReviewService _reviewService;

        public ReviewController(ILogger<ReviewController> logger, ReviewService testService)
        {
            _logger = logger;
            _reviewService = testService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _reviewService.DoStuff(null);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public IActionResult CreateReview(ReviewRequest request) 
        {
            _reviewService.CreateReview(request); // TODO: Try catch a lot: InvalidReviewType + ArgumentNull + ArgumentOutOfRange: BadRequest
            return Ok();
        }

    }
}
