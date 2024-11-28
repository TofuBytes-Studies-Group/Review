
using DnsClient;
using Reviews.API.DTOs;
using Reviews.Domain.Entities;
using Reviews.Domain.Entities.Factories;
using Reviews.Domain.Exceptions;
using Reviews.Infrastructure.Kafka;
using Reviews.Infrastructure.Mappers;
using Reviews.Infrastructure.Repositories;

namespace Reviews.API.Services
{
    public class ReviewService
    {
        private readonly Func<string, IReviewFactory> _factoryResolver;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IKafkaProducer kafkaProducer, Func<string, IReviewFactory> factoryResolver, IReviewRepository reviewRepository)
        {
            _kafkaProducer = kafkaProducer;
            _factoryResolver = factoryResolver;
            _reviewRepository = reviewRepository;
        }

        public async void DoStuff(string type)
        {
            try
            {
                var factory = _factoryResolver(type) ?? throw new InvalidReviewTypeException($"Invalid review type: {type}");
                // Brug KafkaProducer
                await _kafkaProducer.ProduceAsync("topic", "Virker", "From DOSTUFF");
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidReviewTypeException($"Invalid review type: {type}");
            }

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
