
namespace Reviews.Domain.Entities
{
    public class DeliveryAgentReview : Review
    {
        public Guid DeliveryAgentId { get; set; }
        public string DeliveryAgentName { get; set; }

        public DeliveryAgentReview(Guid orderId, string customerUsername, int starRating, string comment, Guid deliveryAgentId, string deliveryAgentName) 
            : base(orderId, customerUsername, starRating, comment)
        {
            if (deliveryAgentId == Guid.Empty)
                throw new ArgumentNullException("DeliveryAgentId is required.");
            if (string.IsNullOrWhiteSpace(deliveryAgentName))
                throw new ArgumentNullException("DeliveryAgentName is required.");
            DeliveryAgentId = deliveryAgentId;
            DeliveryAgentName = deliveryAgentName;
        }
    }
}
