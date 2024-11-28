using Reviews.API.DTOs;
using Reviews.Domain.Entities;

namespace Reviews.API.Services
{
    public interface IReviewService
    {
        Task<Review> CreateReviewAsync(ReviewRequest request);
    }
}
