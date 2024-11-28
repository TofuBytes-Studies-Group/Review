using Microsoft.AspNetCore.Mvc;
using Reviews.API.DTOs;
using Reviews.API.Services;
using Reviews.Domain.Entities;
using Reviews.Domain.Exceptions;

namespace Reviews.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewService _reviewService;

        public ReviewController(ILogger<ReviewController> logger, IReviewService testService)
        {
            _logger = logger;
            _reviewService = testService;
        }

        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(ReviewRequest request)
        {
            try
            {
                _logger.LogInformation("Received a request to create a review: {Request}", request);
                var review = await _reviewService.CreateReviewAsync(request);
                _logger.LogInformation("Review created successfully: {Review}", review);

                return Ok(review);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException occurred while creating review.");
                return BadRequest("One or more required fields are missing.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "ArgumentOutOfRangeException occurred while creating review.");
                return BadRequest("One or more values are out of range.");
            }
            catch (InvalidReviewTypeException ex)
            {
                _logger.LogError(ex, "InvalidReviewTypeException occurred while creating review.");
                return BadRequest("Invalid review type.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating the review.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
