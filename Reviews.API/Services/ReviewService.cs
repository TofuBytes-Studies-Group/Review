
using DnsClient;
using Reviews.API.DTOs;
using Reviews.Domain.Entities;
using Reviews.Domain.Entities.Factories;
using Reviews.Domain.Exceptions;
using Reviews.Infrastructure.Mappers;
using Reviews.Infrastructure.Repositories;

namespace Reviews.API.Services
{
    public class ReviewService : IReviewService
    {
        private readonly Func<string, IReviewFactory> _factoryResolver;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(Func<string, IReviewFactory> factoryResolver, IReviewRepository reviewRepository)
        {
            _factoryResolver = factoryResolver;
            _reviewRepository = reviewRepository;
        }

        public async Task<Review> CreateReviewAsync(ReviewRequest request)
        {
            try
            {
                var factory = _factoryResolver(request.ReviewType) 
                    ?? throw new InvalidReviewTypeException($"Invalid review type: {request.ReviewType}");
                var review = factory.CreateReview(request.OrderId, request.CustomerUsername, 
                    request.StarRating, request.Comment, request.IdOfRevewied, request.NameOfReviewed);
                await _reviewRepository.CreateReviewAsync(review.ToDTO());
                return review;
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidReviewTypeException($"Invalid review type: {request.ReviewType}");
            }
        }
    }
}
