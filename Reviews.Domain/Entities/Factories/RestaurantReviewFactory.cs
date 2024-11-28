namespace Reviews.Domain.Entities.Factories
{
    public class RestaurantReviewFactory : IReviewFactory
    {
        public Review CreateReview(Guid orderId, string username, int rating, string comment, Guid restaurantId, string restaurantName)
        {
            return new RestaurantReview(orderId, username, rating, comment, restaurantId, restaurantName);
        }
    }
}
