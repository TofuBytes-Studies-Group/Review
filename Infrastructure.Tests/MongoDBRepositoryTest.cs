using Infrastructure.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Reviews.Domain.Exceptions;
using Reviews.Infrastructure.Models;
using Reviews.Infrastructure.Repositories;

namespace Infrastructure.Tests
{
    public class MongoDBRepositoryTest : IClassFixture<MongoDBFixture>
    {
        private readonly MongoDBRepository _repository;
        private readonly MongoDBFixture _mongoDBFixture;

        public MongoDBRepositoryTest(MongoDBFixture mongoDBFixture)
        {
            _mongoDBFixture = mongoDBFixture;
            var loggerMock = new Mock<ILogger<MongoDBRepository>>();

            var mongoDBConnection = Options.Create(new MongoDBConnection
            {
                ConnectionString = _mongoDBFixture.ConnectionString,
                DatabaseName = _mongoDBFixture.DatabaseName,
                CollectionName = _mongoDBFixture.CollectionName
            });

            _repository = new MongoDBRepository(mongoDBConnection, loggerMock.Object);
        }

        [Fact]
        public async Task CreateReviewAsync_ValidReviewDTO_ShouldSaveReview()
        {
            // Arrange
            var dto = new ReviewDTO()
            {
                Id = null,
                ReviewType = "restaurant",
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = 1,
                Comment = "Great service!",
                IdOfReviewed = Guid.NewGuid(),
                NameOfReviewed = "Delivery Agent1"
            };

            // Act 
            await _repository.CreateReviewAsync(dto);

            // Assert
            var result = await _repository.GetReviewAsync(dto.OrderId, dto.ReviewType);
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(dto.OrderId, result.OrderId);
            Assert.Equal(dto.CustomerUsername, result.CustomerUsername);
            Assert.Equal(dto.StarRating, result.StarRating);
            Assert.Equal(dto.Comment, result.Comment);
            Assert.Equal(dto.IdOfReviewed, result.IdOfReviewed);
            Assert.Equal(dto.NameOfReviewed, result.NameOfReviewed);
        }

        [Fact]
        public async Task CreateReviewAsync_DuplicateReview_ShouldThrowException()
        {
            // Arrange
            var dto = new ReviewDTO()
            {
                Id = null,
                ReviewType = "restaurant",
                OrderId = Guid.NewGuid(),
                CustomerUsername = "Test User",
                StarRating = 1,
                Comment = "Great service!",
                IdOfReviewed = Guid.NewGuid(),
                NameOfReviewed = "Delivery Agent1"
            };
            await _repository.CreateReviewAsync(dto);

            // Act & Assert 
            var exception = await Assert.ThrowsAsync<MongoWriteException>(
                () => _repository.CreateReviewAsync(dto)
            );
        }

        [Theory]
        [InlineData("restaurant")]
        [InlineData("deliveryAgent")]
        public async Task GetReviewAsync_ReviewNotFoundInDatabase_ShouldThrowReviewNotFoundException(String reviewType)
        {
            // Arrange
            var nonExistentOrderId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ReviewNotFoundException>(
                () => _repository.GetReviewAsync(nonExistentOrderId, reviewType)
            );
            Assert.Equal($"Review of type {reviewType} with OrderId {nonExistentOrderId} not found in database.", exception.Message);
        }

    }
}