using Reviews.API.Services;
using Reviews.Domain.Entities.Factories;
using Reviews.Domain.Entities;
using Moq;
using Reviews.Domain.Exceptions;
using Reviews.API.DTOs;
using Reviews.Infrastructure.Repositories;

namespace API.Test
{
    public class ReviewServiceTest
    {
        private readonly Mock<IReviewRepository> _mockRepo;
        private readonly Mock<Func<string, IReviewFactory>> _mockFactoryResolver;
        private readonly ReviewService _reviewService;

        public ReviewServiceTest()
        {
            _mockRepo = new Mock<IReviewRepository>();
            _mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            _reviewService = new ReviewService(_mockFactoryResolver.Object, _mockRepo.Object);
        }

        [Fact]
        public async void CreateReview_RestaurantReview_ValidRequest_ShouldResolveCorrectReviewFactoryAndCreateReview()
        {
            // Arrange
            var type = "restaurant";

            _mockFactoryResolver
                .Setup(resolver => resolver(type))
                .Returns(new RestaurantReviewFactory());

            var request = new ReviewRequest()
            {
                ReviewType = type,
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = 5,
                Comment = "Great service!",
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = "Test Restaurant",
            };

            // Act
            var review = await _reviewService.CreateReviewAsync(request);

            // Assert
            _mockFactoryResolver.Verify(resolver => resolver(type), Times.Once);

            var restaurantReview = review as RestaurantReview;
            Assert.NotNull(restaurantReview);
            Assert.Equal(request.OrderId, restaurantReview.OrderId);
            Assert.Equal(request.CustomerUsername, restaurantReview.CustomerUsername);
            Assert.Equal(request.StarRating, restaurantReview.StarRating);
            Assert.Equal(request.Comment, restaurantReview.Comment);
            Assert.Equal(request.IdOfRevewied, restaurantReview.RestaurantId);
            Assert.Equal(request.NameOfReviewed, restaurantReview.RestaurantName);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-1)]
        [InlineData(6)]
        [InlineData(10)]
        [InlineData(100)]
        public async void CreateReview_RestaurantReview_IvalidRequest_InvalidStars_ShouldResolveCorrectReviewFactoryAndThrowArgumentOutOfRangeException
            (int starRating)
        {
            // Arrange
            var type = "restaurant";

            _mockFactoryResolver
                .Setup(resolver => resolver(type))
                .Returns(new RestaurantReviewFactory());

            var request = new ReviewRequest()
            {
                ReviewType = type,
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = starRating,
                Comment = "Great service!",
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = "Test Restaurant",
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _reviewService.CreateReviewAsync(request));
            Assert.Equal("Star rating must be between 0 and 5", exception.ParamName);
        }

        [Theory]
        [InlineData(281)]
        [InlineData(282)]
        [InlineData(300)]
        [InlineData(1000)]
        public async void CreateReview_RestaurantReview_IvalidRequest_InvalidComment_TooLong_ShouldResolveCorrectReviewFactoryAndThrowArgumentOutOfRangeException
            (int commentLength)
        {
            // Arrange
            var type = "restaurant";

            _mockFactoryResolver
                .Setup(resolver => resolver(type))
                .Returns(new RestaurantReviewFactory());

            var request = new ReviewRequest()
            {
                ReviewType = type,
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = 1,
                Comment = new string('a', commentLength),
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = "Test Restaurant",
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => 
                _reviewService.CreateReviewAsync(request));
            Assert.Equal("Review cannot be more than 280 characters", exception.ParamName);
        }

        [Fact]
        public async void CreateReview_RestaurantReview_IvalidRequest_NullComment_ShouldResolveCorrectReviewFactoryAndThrowArgumentNullException()
        {
            // Arrange
            var type = "restaurant";

            _mockFactoryResolver
                .Setup(resolver => resolver(type))
                .Returns(new RestaurantReviewFactory());

            var request = new ReviewRequest()
            {
                ReviewType = type,
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = 1,
                Comment = null,
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = "Test Restaurant",
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _reviewService.CreateReviewAsync(request));
            Assert.Equal("Comment is required.", exception.ParamName);
        }

        [Fact]
        public async void CreateReview_DeliveryAgentReview_ShouldResolveCorrectReviewFactoryAndCreateReview()
        {
            // Arrange
            var type = "deliveryAgent";

            _mockFactoryResolver
                .Setup(resolver => resolver(type))
                .Returns(new DeliveryAgentReviewFactory());

            var request = new ReviewRequest()
            {
                ReviewType = type,
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = 5,
                Comment = "Great service!",
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = "Test Deliver Agent"
            };

            // Act
            var review = await _reviewService.CreateReviewAsync(request);

            // Assert
            _mockFactoryResolver.Verify(resolver => resolver(type), Times.Once);

            var agentReview = review as DeliveryAgentReview;
            Assert.NotNull(agentReview);
            Assert.Equal(request.OrderId, agentReview.OrderId);
            Assert.Equal(request.CustomerUsername, agentReview.CustomerUsername);
            Assert.Equal(request.StarRating, agentReview.StarRating);
            Assert.Equal(request.Comment, agentReview.Comment);
            Assert.Equal(request.IdOfRevewied, agentReview.DeliveryAgentId);
            Assert.Equal(request.NameOfReviewed, agentReview.DeliveryAgentName);
        }

        [Fact]
        public async void CreateReview_InvalidReviewType_ShouldThrowInvalidReviewTypeException()
        {
            // Arrange
            var type = "notValid";

            _mockFactoryResolver
                .Setup(resolver => resolver(type))
                .Throws(new KeyNotFoundException());

            var request = new ReviewRequest()
            {
                ReviewType = type,
                OrderId = Guid.NewGuid(),
                CustomerUsername = string.Empty,
                StarRating = 1,
                Comment = string.Empty,
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = string.Empty,
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidReviewTypeException>(() =>
                _reviewService.CreateReviewAsync(request)
                );
            Assert.Equal($"Invalid review type: {type}", exception.Message);

            _mockFactoryResolver.Verify(resolver => resolver(type), Times.Once);
        }

        [Fact]
        public async void CreateReview_NullReviewType_ShouldThrowArgumentNullException()
        {
            // Arrange
            _mockFactoryResolver
                .Setup(resolver => resolver(null))
                .Throws(new ArgumentNullException());

            var request = new ReviewRequest()
            {
                ReviewType = null,
                OrderId = Guid.NewGuid(),
                CustomerUsername = string.Empty,
                StarRating = 1,
                Comment = string.Empty,
                IdOfRevewied = Guid.NewGuid(),
                NameOfReviewed = string.Empty,
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _reviewService.CreateReviewAsync(request)
                );

            _mockFactoryResolver.Verify(resolver => resolver(null), Times.Once);
        }
    }
}
