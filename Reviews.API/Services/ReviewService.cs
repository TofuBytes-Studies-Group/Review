
using DnsClient;
using Reviews.API.DTOs;
using Reviews.Domain.Entities;
using Reviews.Domain.Entities.Factories;
using Reviews.Domain.Exceptions;
using Reviews.Infrastructure.Kafka;

namespace Reviews.API.Services
{
    public class ReviewService
    {
        private readonly Func<string, IReviewFactory> _factoryResolver;
        private readonly IKafkaProducer _kafkaProducer;

        public ReviewService(IKafkaProducer kafkaProducer, Func<string, IReviewFactory> factoryResolver)
        {
            _kafkaProducer = kafkaProducer;
            _factoryResolver = factoryResolver;
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

        public Review CreateReview(ReviewRequest request)
        {
            try
            {
                var factory = _factoryResolver(request.ReviewType) 
                    ?? throw new InvalidReviewTypeException($"Invalid review type: {request.ReviewType}");
                return factory.CreateReview(request.OrderId, request.CustomerUsername, 
                    request.StarRating, request.Comment, request.IdOfRevewied, request.NameOfReviewed);
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidReviewTypeException($"Invalid review type: {request.ReviewType}");
            }
        }
    }
}
