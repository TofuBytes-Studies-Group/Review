using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Reviews.Domain.Aggregates;

namespace Reviews.Infrastructure.Repositories
{
    public class MongoDBRepository : IReviewRepository
    {
        private readonly IMongoCollection<Review> _collection;
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

            _collection = mongoDatabase.GetCollection<Review>(
                mongoConnection.Value.CollectionName);

            _logger.LogInformation("Successfully connected to MongoDB: Database: {DatabaseName}, Collection: {CollectionName}",
                    mongoConnection.Value.DatabaseName, mongoConnection.Value.CollectionName);
        }

        public Task CreateReviewAsync(Review review)
        {
            throw new NotImplementedException();
        }
    }
}
