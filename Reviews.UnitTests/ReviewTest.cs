using Reviews.Domain.Entities;

namespace Reviews.UnitTests
{
    public class ReviewTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void RestarauntReview_New_ValidStarRating_ShouldCreateRestaurnatReviewWithAllAttributes(int starRating)
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = starRating;
            var expectedComment = "Great service!";
            var expectedRestaurantId = Guid.NewGuid();
            var expectedRestaurantName = "Test Restaurant";

            // Act
            var result = new RestaurantReview(
                expectedOrderId, expectedCustomerName, expectedStarRating,
                expectedComment, expectedRestaurantId, expectedRestaurantName
                );

            // Assert
            Assert.IsAssignableFrom<Review>(result);
            Assert.Equal(expectedOrderId, result.OrderId);
            Assert.Equal(expectedCustomerName, result.CustomerUsername);
            Assert.Equal(expectedStarRating, result.StarRating);
            Assert.Equal(expectedComment, result.Comment);
            Assert.Equal(expectedRestaurantId, result.RestaurantId);
            Assert.Equal(expectedRestaurantName, result.RestaurantName);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-1)]
        [InlineData(6)]
        [InlineData(10)]
        [InlineData(100)]
        public void RestarauntReview_New_InvalidStarRating_ShouldThrowArgumentOutOfRangeException(int starRating)
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = starRating;
            var expectedComment = "Great service!";
            var expectedRestaurantId = Guid.NewGuid();
            var expectedRestaurantName = "Test Restaurant";
             
            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new RestaurantReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    expectedComment, expectedRestaurantId, expectedRestaurantName
                    ));
            Assert.Equal("Star rating must be between 0 and 5", exception.ParamName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(140)]
        [InlineData(279)]
        [InlineData(280)]
        public void RestarauntReview_New_ValidCommentLength_ShouldCreateRestaurnatReviewWithAllAttributes(int commentLength)
        { 
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedComment = new string('a', commentLength);
            var expectedRestaurantId = Guid.NewGuid();
            var expectedRestaurantName = "Test Restaurant";

            // Act
            var result = new RestaurantReview(
                expectedOrderId, expectedCustomerName, expectedStarRating,
                expectedComment, expectedRestaurantId, expectedRestaurantName
                );

            // Assert
            Assert.IsAssignableFrom<Review>(result);
            Assert.Equal(expectedOrderId, result.OrderId);
            Assert.Equal(expectedCustomerName, result.CustomerUsername);
            Assert.Equal(expectedStarRating, result.StarRating);
            Assert.Equal(expectedComment, result.Comment);
            Assert.Equal(expectedRestaurantId, result.RestaurantId);
            Assert.Equal(expectedRestaurantName, result.RestaurantName);
        }

        [Theory]
        [InlineData(281)]
        [InlineData(282)]
        [InlineData(300)]
        [InlineData(1000)]
        public void RestarauntReview_New_InvalidCommentLength_TooLong_ShouldThrowArgumentOutOfRangeException(int commentLength)
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedComment = new string('a', commentLength);
            var expectedRestaurantId = Guid.NewGuid();
            var expectedRestaurantName = "Test Restaurant";

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new RestaurantReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    expectedComment, expectedRestaurantId, expectedRestaurantName
                    ));
            Assert.Equal("Review cannot be more than 280 characters", exception.ParamName);
        }

        [Fact]
        public void RestarauntReview_New_InvalidCommentLength_Null_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedRestaurantId = Guid.NewGuid();  
            var expectedRestaurantName = "Test Restaurant";  

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RestaurantReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    null, expectedRestaurantId, expectedRestaurantName
                ));
            Assert.Equal("Comment is required.", exception.ParamName);
        }

        [Fact]
        public void RestarauntReview_New_InvalidCommentLength_Empty_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedRestaurantId = Guid.NewGuid();
            var expectedRestaurantName = "Test Restaurant";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RestaurantReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    string.Empty, expectedRestaurantId, expectedRestaurantName
                ));
            Assert.Equal("Comment is required.", exception.ParamName);
        }

        [Fact]
        public void RestarauntReview_New_InvalidCustomerName_Empty_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedStarRating = 1;
            var expectedComment = "Great service!";
            var expectedRestaurantId = Guid.NewGuid();
            var expectedRestaurantName = "Test Restaurant";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RestaurantReview(
                    expectedOrderId, string.Empty, expectedStarRating,
                    expectedComment, expectedRestaurantId, expectedRestaurantName
                ));
            Assert.Equal("CustomerUsername is required.", exception.ParamName);
        }

        [Fact]
        public void RestarauntReview_New_InvalidRestaurantId_Empty_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedComment = "Great service!";
            var expectedRestaurantId = Guid.Empty;
            var expectedRestaurantName = "Test Restaurant";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RestaurantReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    expectedComment, expectedRestaurantId, expectedRestaurantName
                ));
            Assert.Equal("RestaurantId is required.", exception.ParamName);
        }

        [Fact]
        public void RestarauntReview_New_InvalidOrderId_Empty_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.Empty;
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedComment = "Great service!";
            var expectedRestaurantId = Guid.NewGuid();
            var expectedRestaurantName = "Test Restaurant";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RestaurantReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    expectedComment, expectedRestaurantId, expectedRestaurantName
                ));
            Assert.Equal("OrderId is required.", exception.ParamName);
        }

        [Fact]
        public void RestarauntReview_New_InvalidRestaurantName_Empty_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedComment = "Great service!";
            var expectedRestaurantId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RestaurantReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    expectedComment, expectedRestaurantId, string.Empty
                ));
            Assert.Equal("RestaurantName is required.", exception.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void DeliveryAgentReview_New_ValidStarRating_ShouldCreateRestaurnatReviewWithAllAttributes(int starRating)
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = starRating;
            var expectedComment = "Great service!";
            var expectedAgentId = Guid.NewGuid();
            var expectedAgentName = "Test Delivery Agent";

            // Act
            var result = new DeliveryAgentReview(
                expectedOrderId, expectedCustomerName, expectedStarRating,
                expectedComment, expectedAgentId, expectedAgentName
                );

            // Assert
            Assert.IsAssignableFrom<Review>(result);
            Assert.Equal(expectedOrderId, result.OrderId);
            Assert.Equal(expectedCustomerName, result.CustomerUsername);
            Assert.Equal(expectedStarRating, result.StarRating);
            Assert.Equal(expectedComment, result.Comment);
            Assert.Equal(expectedAgentId, result.DeliveryAgentId);
            Assert.Equal(expectedAgentName, result.DeliveryAgentName);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-1)]
        [InlineData(6)]
        [InlineData(10)]
        [InlineData(100)]
        public void DeliveryAgentReview_New_InvalidStarRating_ShouldThrowArgumentOutOfRangeException(int starRating)
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = starRating;
            var expectedComment = "Great service!";
            var expectedAgentId = Guid.NewGuid();
            var expectedAgentName = "Test Delivery Agent";

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new DeliveryAgentReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    expectedComment, expectedAgentId, expectedAgentName
                ));
            Assert.Equal("Star rating must be between 0 and 5", exception.ParamName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(140)]
        [InlineData(279)]
        [InlineData(280)]
        public void DeliveryAgentReview_New_ValidCommentLength_ShouldCreateRestaurnatReviewWithAllAttributes(int commentLength)
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedComment = new string('a', commentLength); ;
            var expectedAgentId = Guid.NewGuid();
            var expectedAgentName = "Test Delivery Agent";

            // Act
            var result = new DeliveryAgentReview(
                expectedOrderId, expectedCustomerName, expectedStarRating,
                expectedComment, expectedAgentId, expectedAgentName
                );

            // Assert
            Assert.IsAssignableFrom<Review>(result);
            Assert.Equal(expectedOrderId, result.OrderId);
            Assert.Equal(expectedCustomerName, result.CustomerUsername);
            Assert.Equal(expectedStarRating, result.StarRating);
            Assert.Equal(expectedComment, result.Comment);
            Assert.Equal(expectedAgentId, result.DeliveryAgentId);
            Assert.Equal(expectedAgentName, result.DeliveryAgentName);
        }

        [Theory]
        [InlineData(281)]
        [InlineData(282)]
        [InlineData(300)]
        [InlineData(1000)]
        public void DeliveryAgentReview_New_InvalidCommentLength_TooLong_ShouldThrowArgumentOutOfRangeException(int commentLength)
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedComment = new string('a', commentLength); ;
            var expectedAgentId = Guid.NewGuid();
            var expectedAgentName = "Test Delivery Agent";

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new DeliveryAgentReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    expectedComment, expectedAgentId, expectedAgentName
                ));
            Assert.Equal("Review cannot be more than 280 characters", exception.ParamName);
        }

        [Fact]
        public void DeliveryAgentReview_New_InvalidCommentLength_Null_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedAgentId = Guid.NewGuid();
            var expectedAgentName = "Test Delivery Agent";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new DeliveryAgentReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    null, expectedAgentId, expectedAgentName
                ));
            Assert.Equal("Comment is required.", exception.ParamName);
        }


        [Fact]
        public void DeliveryAgentReview_New_InvalidCommentLength_Empty_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedAgentId = Guid.NewGuid();
            var expectedAgentName = "Test Delivery Agent";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new DeliveryAgentReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    string.Empty, expectedAgentId, expectedAgentName
                ));
            Assert.Equal("Comment is required.", exception.ParamName);
        }

        [Fact]
        public void DeliveryAgentReview_New_InvalidCustomerName_Empty_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedStarRating = 1;
            var expectedComment = "Great service!";
            var expectedAgentId = Guid.NewGuid();
            var expectedAgentName = "Test Delivery Agent";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new DeliveryAgentReview(
                    expectedOrderId, string.Empty, expectedStarRating,
                    expectedComment, expectedAgentId, expectedAgentName
                ));
            Assert.Equal("CustomerUsername is required.", exception.ParamName);
        }

        [Fact]
        public void DeliveryAgentReview_New_InvalidDeliveryAgentName_Empty_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedComment = "Great service!";
            var expectedAgentId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new DeliveryAgentReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    expectedComment, expectedAgentId, string.Empty
                ));
            Assert.Equal("DeliveryAgentName is required.", exception.ParamName);
        }

        [Fact]
        public void DeliveryAgentReview_New_InvalidRestaurantId_Empty_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.NewGuid();
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedComment = "Great service!";
            var expectedAgentId = Guid.Empty;
            var expectedAgentName = "Test Delivery Agent";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new DeliveryAgentReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    expectedComment, expectedAgentId, expectedAgentName
                ));
            Assert.Equal("DeliveryAgentId is required.", exception.ParamName);
        }

        [Fact]
        public void DeliveryAgentReview_New_InvalidOrderId_Empty_ShouldThrowArgumentNullException()
        {
            // Arrange 
            var expectedOrderId = Guid.Empty;
            var expectedCustomerName = "Test User";
            var expectedStarRating = 1;
            var expectedComment = "Great service!";
            var expectedAgentId = Guid.NewGuid();
            var expectedAgentName = "Test Delivery Agent";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new DeliveryAgentReview(
                    expectedOrderId, expectedCustomerName, expectedStarRating,
                    expectedComment, expectedAgentId, expectedAgentName
                ));
            Assert.Equal("OrderId is required.", exception.ParamName);
        }
    }
}