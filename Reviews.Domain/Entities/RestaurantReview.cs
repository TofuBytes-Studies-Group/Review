
namespace Reviews.Domain.Entities
{
    public class RestaurantReview : Review
    {
        public Guid RestaurantId { get; set; }
        public string RestaurantName { get; set; }

        public RestaurantReview(Guid orderId, string customerUsername, int starRating, string comment, Guid restaurantId, string restaurantName)
            : base(orderId, customerUsername, starRating, comment)
        {
            if (restaurantId == Guid.Empty)
                throw new ArgumentNullException("RestaurantId is required.");
            if (string.IsNullOrWhiteSpace(restaurantName))
                throw new ArgumentNullException("RestaurantName is required.");
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
        }
    }
}
