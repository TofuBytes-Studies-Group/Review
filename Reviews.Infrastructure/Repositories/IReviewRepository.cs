using Reviews.Infrastructure.Models;

namespace Reviews.Infrastructure.Repositories
{
    public interface IReviewRepository
    {
        Task CreateReviewAsync(ReviewDTO review);
    }
}
