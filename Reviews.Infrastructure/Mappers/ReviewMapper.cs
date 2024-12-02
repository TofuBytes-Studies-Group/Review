using Reviews.Domain.Entities;
using Reviews.Infrastructure.Models;
using Reviews.Domain.Entities.Factories;
using Reviews.Domain.Exceptions;

namespace Reviews.Infrastructure.Mappers
{
    public static class ReviewMapper
    {
        public static ReviewDTO ToDTO(this Review domain)
        {
            return domain switch
            {
                DeliveryAgentReview deliveryAgentReview => new ReviewDTO
                {
                    ReviewType = "deliveryAgent",
                    OrderId = deliveryAgentReview.OrderId,
                    CustomerUsername = deliveryAgentReview.CustomerUsername,
                    StarRating = deliveryAgentReview.StarRating,
                    Comment = deliveryAgentReview.Comment,
                    IdOfReviewed = deliveryAgentReview.DeliveryAgentId,
                    NameOfReviewed = deliveryAgentReview.DeliveryAgentName
                },
                RestaurantReview restaurantReview => new ReviewDTO
                {
                    ReviewType = "restaurant",
                    OrderId = restaurantReview.OrderId,
                    CustomerUsername = restaurantReview.CustomerUsername,
                    StarRating = restaurantReview.StarRating,
                    Comment = restaurantReview.Comment,
                    IdOfReviewed = restaurantReview.RestaurantId,
                    NameOfReviewed = restaurantReview.RestaurantName
                },
                _ => throw new InvalidReviewTypeException($"Invalid review type: {domain.GetType().Name}")
            };
        }

        public static Review ToDomain(this ReviewDTO dto, IReviewFactory factory)
        {
            return factory.CreateReview(
                    dto.OrderId,
                    dto.CustomerUsername,
                    dto.StarRating,
                    dto.Comment,
                    dto.IdOfReviewed,
                    dto.NameOfReviewed
                    );
        }
    }
}
