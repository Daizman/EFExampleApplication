using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace EFExampleApplication.Tests;

public class ReviewServiceTests : TestServiceBase
{
    private readonly ReviewService _sut;

    public ReviewServiceTests() : base()
    {
        var logger = new NullLogger<ReviewService>();
        _sut = new(Context, Mapper, logger);
    }

    [Fact]
    public void AddReviewV1_WhenCorrectInput_SuccessfullyCreated()
    {
        // Arrange
        var dto = new CreateReviewDto(
            Content: "Good",
            Score: 4, 
            MovieId: FakeApplicationDbContextFactory.MovieId, 
            UserId: FakeApplicationDbContextFactory.UserAId
        );

        // Act
        var result = _sut.AddReview(dto);

        // Assert
        var created = Context.Reviews.Find(result);
        Assert.NotNull(created);
        Assert.Equal("Good", created.Content);
        Assert.Equal(4, created.Score);
    }

    [Fact]
    public void DeleteReview_WhenReviewExists_RemovesSuccessfully()
    {
        // Arrange
        // Act
        _sut.DeleteReview(FakeApplicationDbContextFactory.ReviewIdToDelete);

        // Assert
        var deleted = Context.Reviews.Find(FakeApplicationDbContextFactory.ReviewIdToDelete);
        Assert.Null(deleted);
    }

    [Fact]
    public void DeleteReview_WhenReviewNotFound_ThrowsReviewNotFoundException()
    {
        // Arrange
        // Act
        var act = () => _sut.DeleteReview(id: 9999);

        // Assert
        Assert.Throws<ReviewNotFoundException>(act);
    }
}
