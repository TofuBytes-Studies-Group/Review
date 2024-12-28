namespace Reviews.Domain.Exceptions
{
    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException(String message) : base(message) { }
    }
}
