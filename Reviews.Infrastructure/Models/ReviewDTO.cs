using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Reviews.Infrastructure.Models
{
    public class ReviewDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string ReviewType { get; set; }
        public required Guid OrderId { get; set; }
        public required string CustomerUsername { get; set; }
        public required int StarRating { get; set; }

        public required string Comment { get; set; }
        public required Guid IdOfReviewed { get; set; }
        public required string NameOfReviewed { get; set; }
    }
}
