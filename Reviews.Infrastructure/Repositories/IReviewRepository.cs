using Reviews.Domain.Aggregates;

namespace Reviews.Infrastructure.Repositories
{
    public interface IReviewRepository
    {
        Task CreateReviewAsync(Review review);
    }
}
