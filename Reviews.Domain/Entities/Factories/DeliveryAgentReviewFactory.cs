namespace Reviews.Domain.Entities.Factories
{
    public class DeliveryAgentReviewFactory : IReviewFactory
    {
        public Review CreateReview(Guid orderId, string username, int rating, string comment, Guid agentId, string agentName)
        {
            return new DeliveryAgentReview(orderId, username, rating, comment, agentId, agentName);
        }
    }
}
