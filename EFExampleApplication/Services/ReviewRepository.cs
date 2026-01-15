using EFExampleApplication.Abstractions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Services;

public class ReviewRepository(IApplicationDbContext applicationDbContext) : IReviewRepository
{
    private readonly DbSet<Review> _reviews = applicationDbContext.Reviews;

    public IReadOnlyList<Review> GetReviews(int movieId)
    {
        return _reviews.Where(r => r.MovieId == movieId).AsNoTracking().ToList();
    }

    // Тут возникает проблема с тем, что мы в каких-то методах репозитория,
    // хотим отдавать модель целиком, с User и Movie, а в каких-то нет.
    // В итоге:
    // - Нам снаружи везде нужно проверять, есть User и Movie у полученного объекта или нет.
    //   Т.е. абстракция не работает, нам нужно знать, был ли Include в реализации или нет.
    // - Мы на каждый запрос, где есть Include достаем все колонки из БД, даже если нам нужна только одна сущность.
    public Review? GetReview(int movieId, int reviewId)
    {
        return _reviews
            .AsNoTracking()
            .FirstOrDefault(r => r.Id == reviewId && r.MovieId == movieId);
    }

    public int AddReview(Review newReview)
    {
        _reviews.Add(newReview);

        applicationDbContext.SaveChanges();

        return newReview.Id;
    }

    public bool UpdateReview(int reviewId, string? content, int? score)
    {
        var review = _reviews.FirstOrDefault(r => r.Id == reviewId);

        if (review is null)
        {
            return false;
        }

        review.Content = content ?? review.Content;
        review.Score = score ?? review.Score;

        applicationDbContext.SaveChanges();

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

        applicationDbContext.SaveChanges();

        return true;
    }
}
