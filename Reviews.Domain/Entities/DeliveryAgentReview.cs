
namespace Reviews.Domain.Entities
{
    public class DeliveryAgentReview : Review
    {
        public Guid DeliveryAgentId { get; set; }
        public string DeliveryAgentName { get; set; }

        public DeliveryAgentReview(Guid orderId, string customerUsername, int starRating, string comment, Guid deliveryAgentId, string deliveryAgentName) 
            : base(orderId, customerUsername, starRating, comment)
        {
            DeliveryAgentId = deliveryAgentId;
            DeliveryAgentName = deliveryAgentName;
        }
    }
}
