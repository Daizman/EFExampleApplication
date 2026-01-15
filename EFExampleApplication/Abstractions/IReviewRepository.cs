using EFExampleApplication.Models;

namespace EFExampleApplication.Abstractions;

public interface IReviewRepository
{
    IReadOnlyList<Review> GetReviews(int movieId);
    Review? GetReview(int movieId, int reviewId);
    int AddReview(Review newReview);
    bool UpdateReview(int reviewId, string? content, int? score);
    bool DeleteReview(int reviewId);
}
