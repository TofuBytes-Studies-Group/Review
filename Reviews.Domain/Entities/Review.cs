﻿namespace Reviews.Domain.Entities
{
    public abstract class Review
    {
        public Guid OrderId { get; set; }
        public string CustomerUsername { get; set; }
        private int _starRating;

        public int StarRating
        {
            get => _starRating;
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentOutOfRangeException("Star rating must be between 0 and 5");
                _starRating = value;
            }
        }

        private string _comment = string.Empty;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value.Length > 280)
                    throw new ArgumentOutOfRangeException("Review cannot be more than 280 characters");
                _comment = value;
            }
        }

        protected Review(Guid orderId, string customerUsername, int starRating, string comment)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentNullException("OrderId is required.");
            if (string.IsNullOrWhiteSpace(customerUsername))
                throw new ArgumentNullException("CustomerUsername is required.");
            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentNullException("Comment is required.");

            OrderId = orderId;
            CustomerUsername = customerUsername;
            StarRating = starRating;
            Comment = comment;
        }
    }
}