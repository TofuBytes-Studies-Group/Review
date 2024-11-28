using Reviews.API.Services;
using Reviews.Domain.Entities.Factories;
using Reviews.Domain.Entities;
using Reviews.Infrastructure.Kafka;
using Moq;
using Reviews.Domain.Exceptions;
using Reviews.API.DTOs;
using Reviews.Infrastructure.Repositories;

namespace API.Test
{
    public class ReviewServiceTest
    {
        [Fact]
        public async void CreateReview_RestaurantReview_ValidRequest_ShouldResolveCorrectReviewFactoryAndCreateReview()
        {
            // Arrange
            var mockKafkaProducer = new Mock<IKafkaProducer>();
            var mockRepo = new Mock<IReviewRepository>();

            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver
                .Setup(resolver => resolver("restaurant"))
                .Returns(new RestaurantReviewFactory());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object, mockRepo.Object);

            var type = "restaurant";

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
            var review = await reviewService.CreateReviewAsync(request);

            // Assert
            mockFactoryResolver.Verify(resolver => resolver(type), Times.Once);

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
            var mockKafkaProducer = new Mock<IKafkaProducer>();
            var mockRepo = new Mock<IReviewRepository>();

            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver
                .Setup(resolver => resolver("restaurant"))
                .Returns(new RestaurantReviewFactory());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object, mockRepo.Object);

            var type = "restaurant";

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
                reviewService.CreateReviewAsync(request));
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
            var mockKafkaProducer = new Mock<IKafkaProducer>();
            var mockRepo = new Mock<IReviewRepository>();

            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver
                .Setup(resolver => resolver("restaurant"))
                .Returns(new RestaurantReviewFactory());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object, mockRepo.Object);

            var type = "restaurant";

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
                reviewService.CreateReviewAsync(request));
            Assert.Equal("Review cannot be more than 280 characters", exception.ParamName);
        }

        [Fact]
        public async void CreateReview_RestaurantReview_IvalidRequest_NullComment_ShouldResolveCorrectReviewFactoryAndThrowArgumentNullException()
        {
            // Arrange
            var mockKafkaProducer = new Mock<IKafkaProducer>();
            var mockRepo = new Mock<IReviewRepository>();

            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver
                .Setup(resolver => resolver("restaurant"))
                .Returns(new RestaurantReviewFactory());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object, mockRepo.Object);

            var type = "restaurant";

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
                reviewService.CreateReviewAsync(request));
            Assert.Equal("Comment is required.", exception.ParamName);
        }

        [Fact]
        public async void CreateReview_DeliveryAgentReview_ShouldResolveCorrectReviewFactoryAndCreateReview()
        {
            // Arrange
            var mockKafkaProducer = new Mock<IKafkaProducer>();
            var mockRepo = new Mock<IReviewRepository>();

            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver
                .Setup(resolver => resolver("deliveryAgent"))
                .Returns(new DeliveryAgentReviewFactory());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object, mockRepo.Object);

            var type = "deliveryAgent";

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
            var review = await reviewService.CreateReviewAsync(request);

            // Assert
            mockFactoryResolver.Verify(resolver => resolver(type), Times.Once);

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
            var mockKafkaProducer = new Mock<IKafkaProducer>();
            var mockRepo = new Mock<IReviewRepository>();

            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver
                .Setup(resolver => resolver("notValid"))
                .Throws(new KeyNotFoundException());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object, mockRepo.Object);

            var type = "notValid";

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
                reviewService.CreateReviewAsync(request)
                );
            Assert.Equal($"Invalid review type: {type}", exception.Message);

            mockFactoryResolver.Verify(resolver => resolver(type), Times.Once);
        }

        [Fact]
        public async void CreateReview_NullReviewType_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockKafkaProducer = new Mock<IKafkaProducer>();
            var mockRepo = new Mock<IReviewRepository>();

            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver
                .Setup(resolver => resolver(null))
                .Throws(new ArgumentNullException());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object, mockRepo.Object);

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
                reviewService.CreateReviewAsync(request)
                );

            mockFactoryResolver.Verify(resolver => resolver(null), Times.Once);
        }
    }
}
