using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reviews.API.Controllers;
using Reviews.API.DTOs;
using Reviews.API.Services;
using Reviews.Domain.Entities;
using Reviews.Domain.Entities.Factories;
using Reviews.Infrastructure.Kafka;
using Reviews.Infrastructure.Repositories;

namespace API.Test
{
    public class ReviewControllerTest
    {
        private readonly Mock<IKafkaProducer> _mockKafkaProducer;
        private readonly Mock<IReviewRepository> _mockRepo;
        private readonly Mock<Func<string, IReviewFactory>> _mockFactoryResolver;
        private readonly Mock<ILogger<ReviewController>> _mockLogger;

        private readonly ReviewService _reviewService;
        private readonly ReviewController _controller;

        public ReviewControllerTest()
        {
            _mockKafkaProducer = new Mock<IKafkaProducer>();
            _mockRepo = new Mock<IReviewRepository>();
            _mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            _mockLogger = new Mock<ILogger<ReviewController>>();

            _reviewService = new ReviewService(_mockKafkaProducer.Object, _mockFactoryResolver.Object, _mockRepo.Object);
            _controller = new ReviewController(_mockLogger.Object, _reviewService);
        }

        [Fact]
        public async Task CreateReview_RestaurantReview_ShouldReturnOkResult()
        {
            // Arrange
            var type = "restaurant";

            _mockFactoryResolver
                .Setup(resolver => resolver(type))
                .Returns(new RestaurantReviewFactory());

            var reviewRequest = new ReviewRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = 5,
                Comment = "Great service!",
                ReviewType = type,
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = "Test Restaurant"
            };

            // Act
            var result = await _controller.CreateReview(reviewRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<RestaurantReview>(okResult.Value);
            Assert.Equal(reviewRequest.OrderId, returnValue.OrderId);
            Assert.Equal(reviewRequest.CustomerUsername, returnValue.CustomerUsername);
            Assert.Equal(reviewRequest.StarRating, returnValue.StarRating);
            Assert.Equal(reviewRequest.Comment, returnValue.Comment);
            Assert.Equal(reviewRequest.IdOfRevewied, returnValue.RestaurantId);
            Assert.Equal(reviewRequest.NameOfReviewed, returnValue.RestaurantName);
        }

        [Fact]
        public async Task CreateReview_DeliveryAgentReview_ShouldReturnOkResult()
        {
            // Arrange
            var type = "deliveryAgent";

            _mockFactoryResolver
                .Setup(resolver => resolver(type))
                .Returns(new DeliveryAgentReviewFactory());

            var reviewRequest = new ReviewRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = 5,
                Comment = "Great service!",
                ReviewType = type,
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = "Test Delivery Agent"
            };

            // Act
            var result = await _controller.CreateReview(reviewRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<DeliveryAgentReview>(okResult.Value);
            Assert.Equal(reviewRequest.OrderId, returnValue.OrderId);
            Assert.Equal(reviewRequest.CustomerUsername, returnValue.CustomerUsername);
            Assert.Equal(reviewRequest.StarRating, returnValue.StarRating);
            Assert.Equal(reviewRequest.Comment, returnValue.Comment);
            Assert.Equal(reviewRequest.IdOfRevewied, returnValue.DeliveryAgentId);
            Assert.Equal(reviewRequest.NameOfReviewed, returnValue.DeliveryAgentName);
        }

        [Fact]
        public async Task CreateReview_RestaurantReview_ArgumentNullException_ShouldReturnBadRequest()
        {
            // Arrange
            var type = "restaurant";

            _mockFactoryResolver
                .Setup(resolver => resolver(type))
                .Returns(new RestaurantReviewFactory());

            var reviewRequest = new ReviewRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerUsername = null,
                StarRating = 5,
                Comment = string.Empty,
                ReviewType = type,
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = string.Empty
            };

            // Act
            var result = await _controller.CreateReview(reviewRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("One or more required fields are missing.", badRequestResult.Value);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-1)]
        [InlineData(6)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task CreateReview_RestaurantReview_ArgumentOutOfRangeException_ShouldReturnBadRequest(int starRating)
        {
            // Arrange
            var type = "restaurant";

            _mockFactoryResolver
                .Setup(resolver => resolver(type))
                .Returns(new RestaurantReviewFactory());

            var reviewRequest = new ReviewRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = starRating,
                Comment = "Great service!",
                ReviewType = type,
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = "Test Restaurant"
            };

            // Act
            var result = await _controller.CreateReview(reviewRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("One or more values are out of range.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateReview_RestaurantReview_InvalidReviewType_ShouldReturnBadRequest()
        {
            // Arrange
            var type = "invalid"; //Invalid type

            var reviewRequest = new ReviewRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerUsername = "testuser",
                StarRating = 4,
                Comment = "Excellent service!",
                ReviewType = type,
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = "Test Restaurant"
            };

            // Act
            var result = await _controller.CreateReview(reviewRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid review type.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateReview_RestaurantReview_UnhandledException_ShouldReturnInternalServerError()
        {
            // Arrange
            var mockReviewService = new Mock<IReviewService>(); //With mock service here to create another unexpected exception
            var mockLogger = new Mock<ILogger<ReviewController>>();
            var controller = new ReviewController(mockLogger.Object, mockReviewService.Object);

            var reviewRequest = new ReviewRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerUsername = "testuser",
                StarRating = 5,
                Comment = "Amazing!",
                ReviewType = "restaurant",
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = "Test Restaurant"
            };

            mockReviewService.Setup(service => service.CreateReviewAsync(reviewRequest))
                .ThrowsAsync(new Exception("Unhandled error"));

            // Act
            var result = await controller.CreateReview(reviewRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An unexpected error occurred. Please try again later.", statusCodeResult.Value);
        }
    }
}
