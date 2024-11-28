namespace Reviews.API.DTOs
{
    public class ReviewRequest
    {
        public required string ReviewType { get; set; }
        public required Guid OrderId { get; set; }
        public required string CustomerUsername { get; set; }
        public required int StarRating { get; set; }

        public required string Comment { get; set; }
        public required Guid IdOfRevewied { get; set; }
        public required string NameOfReviewed { get; set; }
    }
}
