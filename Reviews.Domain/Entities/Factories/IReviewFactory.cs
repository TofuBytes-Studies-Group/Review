namespace Reviews.Domain.Entities.Factories
{
    public interface IReviewFactory
    {
        Review CreateReview(
            Guid orderId,
            string username,
            int rating,
            string comment,
            Guid entityId,
            string entityName
            );
    }
}
