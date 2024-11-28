
using DnsClient;
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
            catch (Exception)
            {
                throw new InvalidReviewTypeException($"Invalid review type: {type}");
            }

        }

        public Review CreateReview(Guid orderId, string customerUsername, int rating,
            string comment, Guid entityId, string entityName, string type)
        {
            try
            {
                var factory = _factoryResolver(type) ?? throw new InvalidReviewTypeException($"Invalid review type: {type}");
                return factory.CreateReview(orderId, customerUsername, rating, comment, entityId, entityName);
            }
            catch (Exception)
            {
                throw new InvalidReviewTypeException($"Invalid review type: {type}");
            }
        }
    }
}
