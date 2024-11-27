using Reviews.Domain.Entities;

namespace Reviews.Infrastructure.Repositories
{
    public interface IReviewRepository
    {
        Task CreateReviewAsync(Review review);
    }
}
