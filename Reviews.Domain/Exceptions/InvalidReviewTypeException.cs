namespace Reviews.Domain.Exceptions
{
    public class InvalidReviewTypeException : Exception
    {
        public InvalidReviewTypeException(string message) : base(message)
        {
        }
    }
}
