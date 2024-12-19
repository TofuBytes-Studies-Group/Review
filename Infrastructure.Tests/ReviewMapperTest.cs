using Reviews.Domain.Entities.Factories;
using Reviews.Infrastructure.Models;
using Reviews.Infrastructure.Mappers;
using Reviews.Domain.Entities;

namespace Infrastructure.Tests
{
    public class ReviewMapperTest
    {
        [Fact]
        public void ToDomain_RestaurantReview_ShouldReturnCorrectReviewObject()
        {
            // Arrange
            var dto = new ReviewDTO()
            {
                Id = "1",
                ReviewType = "restaurant",
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = 1,
                Comment = "Great service!",
                IdOfReviewed = Guid.NewGuid(),
                NameOfReviewed = "Delivery Agent1"
            };
            var factory = new RestaurantReviewFactory();

            // Act
            var domain = dto.ToDomain(factory);

            // Assert
            Assert.NotNull(domain);
            Assert.IsAssignableFrom<Review>(domain);
            Assert.Equal(dto.OrderId, domain.OrderId);
            Assert.Equal(dto.CustomerUsername, domain.CustomerUsername);
            Assert.Equal(dto.StarRating, domain.StarRating);
            Assert.Equal(dto.Comment, domain.Comment);
            Assert.IsType<RestaurantReview>(domain);
            RestaurantReview restaruantReview = (RestaurantReview)domain;
            Assert.Equal(dto.IdOfReviewed, restaruantReview.RestaurantId);
            Assert.Equal(dto.NameOfReviewed, restaruantReview.RestaurantName);
        }

        [Fact]
        public void ToDomain_DeliveryAgentReview_ShouldReturnCorrectReviewObject()
        {
            // Arrange
            var dto = new ReviewDTO()
            {
                Id = "1",
                ReviewType = "deliveryAgent",
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = 1,
                Comment = "Great service!",
                IdOfReviewed = Guid.NewGuid(),
                NameOfReviewed = "Test Agent"
            };
            var factory = new DeliveryAgentReviewFactory();

            // Act
            var domain = dto.ToDomain(factory);

            // Assert
            Assert.NotNull(domain);
            Assert.IsAssignableFrom<Review>(domain);
            Assert.Equal(dto.OrderId, domain.OrderId);
            Assert.Equal(dto.CustomerUsername, domain.CustomerUsername);
            Assert.Equal(dto.StarRating, domain.StarRating);
            Assert.Equal(dto.Comment, domain.Comment);
            Assert.IsType<DeliveryAgentReview>(domain);
            DeliveryAgentReview deliveryAgentReview = (DeliveryAgentReview)domain;
            Assert.Equal(dto.IdOfReviewed, deliveryAgentReview.DeliveryAgentId);
            Assert.Equal(dto.NameOfReviewed, deliveryAgentReview.DeliveryAgentName);
        }

        [Fact]
        public void ToDTO_RestaurantReview_ShouldReturnCorrectReviewDTOObject()
        {
            // Arrange 
            var expectedType = "restaurant";

            var domain = new RestaurantReview(
                Guid.NewGuid(),
                "Test User",
                1,
                "Great service!", 
                Guid.NewGuid(), 
                "Test Restaurant"
                );

            // Act
            var dto = domain.ToDTO();

            // Assert
            Assert.NotNull(dto);
            Assert.Null(dto.Id);
            Assert.Equal(expectedType, dto.ReviewType);
            Assert.Equal(domain.OrderId, dto.OrderId);
            Assert.Equal(domain.CustomerUsername, dto.CustomerUsername);
            Assert.Equal(domain.StarRating, dto.StarRating);
            Assert.Equal(domain.Comment, dto.Comment);
            Assert.Equal(domain.RestaurantId, dto.IdOfReviewed);
            Assert.Equal(domain.RestaurantName, dto.NameOfReviewed);
        }

        [Fact]
        public void ToDTO_DeliveryAgentReview_ShouldReturnCorrectReviewDTOObject()
        {
            // Arrange 
            var expectedType = "deliveryAgent";

            var domain = new DeliveryAgentReview(
                Guid.NewGuid(),
                "Test User",
                1,
                "Great service!",
                Guid.NewGuid(),
                "Test Agent"
                );

            // Act
            var dto = domain.ToDTO();

            // Assert
            Assert.NotNull(dto);
            Assert.Null(dto.Id);
            Assert.Equal(expectedType, dto.ReviewType);
            Assert.Equal(domain.OrderId, dto.OrderId);
            Assert.Equal(domain.CustomerUsername, dto.CustomerUsername);
            Assert.Equal(domain.StarRating, dto.StarRating);
            Assert.Equal(domain.Comment, dto.Comment);
            Assert.Equal(domain.DeliveryAgentId, dto.IdOfReviewed);
            Assert.Equal(domain.DeliveryAgentName, dto.NameOfReviewed);
        }
    }
}