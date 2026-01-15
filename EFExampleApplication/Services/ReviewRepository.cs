using EFExampleApplication.Abstractions;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class ReviewRepository : IReviewRepository
{
    private readonly List<Review> _reviews = new();

    public IReadOnlyList<Review> GetReviews(int movieId)
    {
        return _reviews.Where(r => r.MovieId == movieId).ToList();
    }

    public Review? GetReview(int movieId, int reviewId)
    {
        return _reviews.FirstOrDefault(r => r.Id == reviewId && r.MovieId == movieId);
    }

    public int AddReview(Review newReview)
    {
        newReview.Id = _reviews.Count + 1;
        _reviews.Add(newReview);

        return newReview.Id;
    }

    public bool UpdateReview(int reviewId, string? content, int? score)
    {
        var review = GetReviewByIdAndThrowIfNotFound(reviewId);

        if (review is null)
        {
            return false;
        }

        review.Content = content ?? review.Content;
        review.Score = score ?? review.Score;

        return true;
    }

    public bool DeleteReview(int reviewId)
    {
        var review = _reviews.FirstOrDefault(r => r.Id == reviewId);
        if (review is null)
        {
            return false;
        }

        _reviews.Remove(review);
        return true;
    }

    private Review GetReviewByIdAndThrowIfNotFound(int id)
    {
        var review = _reviews.FirstOrDefault(r => r.Id == id);
        if (review is null)
        {
            throw new ReviewNotFoundException(id);
        }

        return review;
    }
}
