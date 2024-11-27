
using Reviews.Infrastructure.Kafka;

namespace Reviews.API.Services
{
    public class ReviewService  
    {
        private readonly KafkaProducer _kafkaProducer;
        public ReviewService(KafkaProducer kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        public async void DoStuff()
        {
            // Brug KafkaProducer
            await _kafkaProducer.ProduceAsync("topic", "Virker", "From DOSTUFF");
        }
    }
}
