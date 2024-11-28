using Reviews.API.Services;
using Reviews.Domain.Entities.Factories;
using Reviews.Domain.Entities;
using Reviews.Infrastructure.Kafka;
using Moq;
using Reviews.Domain.Exceptions;

namespace API.Test
{
    public class ReviewServiceTest
    {
        [Fact]
        public void CreateReview_RestaurantReview_ShouldResolveCorrectReviewFactoryAndCreateReview()
        {
            // Arrange
            var mockKafkaProducer = new Mock<IKafkaProducer>();
             
            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver 
                .Setup(resolver => resolver("restaurant"))
                .Returns(new RestaurantReviewFactory());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object);

            var orderId = Guid.NewGuid();
            var username = "Test User";
            var rating = 5;
            var comment = "Great service!";
            var entityId = Guid.NewGuid();
            var entityName = "Test Restaurant";
            var type = "restaurant";

            // Act
            var review = reviewService.CreateReview(orderId, username, rating, comment, entityId, entityName, type);

            // Assert
            mockFactoryResolver.Verify(resolver => resolver(type), Times.Once);

            var restaurantReview = review as RestaurantReview;
            Assert.NotNull(restaurantReview);
            Assert.Equal(orderId, restaurantReview.OrderId);
            Assert.Equal(username, restaurantReview.CustomerUsername);
            Assert.Equal(rating, restaurantReview.StarRating);
            Assert.Equal(comment, restaurantReview.Comment);
            Assert.Equal(entityId, restaurantReview.RestaurantId);
            Assert.Equal(entityName, restaurantReview.RestaurantName);
        }

        [Fact]
        public void CreateReview_DeliveryAgentReview_ShouldResolveCorrectReviewFactoryAndCreateReview()
        {
            // Arrange
            var mockKafkaProducer = new Mock<IKafkaProducer>();

            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver
                .Setup(resolver => resolver("deliveryAgent"))
                .Returns(new DeliveryAgentReviewFactory());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object);

            var orderId = Guid.NewGuid();
            var username = "Test User";
            var rating = 1;
            var comment = "Great service!";
            var entityId = Guid.NewGuid();
            var entityName = "Test Deliver Agent";
            var type = "deliveryAgent";

            // Act
            var review = reviewService.CreateReview(orderId, username, rating, comment, entityId, entityName, type);

            // Assert
            mockFactoryResolver.Verify(resolver => resolver(type), Times.Once);

            var agentReview = review as DeliveryAgentReview;
            Assert.NotNull(agentReview);
            Assert.Equal(orderId, agentReview.OrderId);
            Assert.Equal(username, agentReview.CustomerUsername);
            Assert.Equal(rating, agentReview.StarRating);
            Assert.Equal(comment, agentReview.Comment);
            Assert.Equal(entityId, agentReview.DeliveryAgentId);
            Assert.Equal(entityName, agentReview.DeliveryAgentName);
        }

        [Fact]
        public void CreateReview_InvalidReviewType_ShouldThrowInvalidReviewTypeException()
        {
            // Arrange
            var mockKafkaProducer = new Mock<IKafkaProducer>();

            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver
                .Setup(resolver => resolver("notValid"))
                .Throws(new KeyNotFoundException());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object);

            var orderId = Guid.NewGuid();
            var username = string.Empty;
            var rating = 1;
            var comment = string.Empty;
            var entityId = Guid.NewGuid();
            var entityName = string.Empty;
            var type = "notValid";

            // Act & Assert
            var exception = Assert.Throws<InvalidReviewTypeException>(() => 
                reviewService.CreateReview(orderId, username, rating, comment, entityId, entityName, type)
                );
            Assert.Equal($"Invalid review type: {type}", exception.Message);

            mockFactoryResolver.Verify(resolver => resolver(type), Times.Once);
        }

        [Fact]
        public void CreateReview_NullReviewType_ShouldThrowInvalidReviewTypeException()
        {
            // Arrange
            var mockKafkaProducer = new Mock<IKafkaProducer>();

            var mockFactoryResolver = new Mock<Func<string, IReviewFactory>>();
            mockFactoryResolver
                .Setup(resolver => resolver(null))
                .Throws (new ArgumentNullException());

            var reviewService = new ReviewService(mockKafkaProducer.Object, mockFactoryResolver.Object);

            var orderId = Guid.NewGuid();
            var username = string.Empty;
            var rating = 1;
            var comment = string.Empty;
            var entityId = Guid.NewGuid();
            var entityName = string.Empty;

            // Act & Assert
            var exception = Assert.Throws<InvalidReviewTypeException>(() =>
                reviewService.CreateReview(orderId, username, rating, comment, entityId, entityName, null)
                );
            Assert.Equal("Invalid review type: ", exception.Message);

            mockFactoryResolver.Verify(resolver => resolver(null), Times.Once);
        }
    }
}
