using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Reviews.Infrastructure.Models;
using Reviews.Domain.Exceptions;

namespace Reviews.Infrastructure.Repositories
{
    public class MongoDBRepository : IReviewRepository
    {
        private readonly IMongoCollection<ReviewDTO> _collection;
        private readonly ILogger<MongoDBRepository> _logger;

        private static bool _guidSerializerRegistered = false;

        public MongoDBRepository(IOptions<MongoDBConnection> mongoConnection, ILogger<MongoDBRepository> logger)
        {
            _logger = logger;

            // Needed to serialize the guid TODO: Find out if review needs a GUID
            if (!_guidSerializerRegistered)
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                _guidSerializerRegistered = true;
            }

            var mongoClient = new MongoClient(
                mongoConnection.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoConnection.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<ReviewDTO>(
                mongoConnection.Value.CollectionName);

            _logger.LogInformation("Successfully connected to MongoDB: Database: {DatabaseName}, Collection: {CollectionName}",
                    mongoConnection.Value.DatabaseName, mongoConnection.Value.CollectionName);
        }

        public async Task CreateReviewAsync(ReviewDTO review)
        {
            try
            {
                await _collection.InsertOneAsync(review);
                _logger.LogInformation("Successfully inserted {ReviewType} review for order with id: {OrderId}", review.ReviewType, review.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting {ReviewType} review for order with id: {OrderId}", review.ReviewType, review.OrderId);
                throw;
            }
        }

        public async Task<ReviewDTO> GetReviewAsync(Guid orderId, String reviewType)
        {
            var result = await _collection.Find(o => o.OrderId == orderId && o.ReviewType == reviewType).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new ReviewNotFoundException($"Review of type {reviewType} with OrderId {orderId} not found in database.");
            }
            return result;
        }
    }
}
