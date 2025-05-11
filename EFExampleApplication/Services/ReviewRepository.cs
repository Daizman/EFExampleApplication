using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Database;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Services;

public class ReviewRepository(
    IMapper mapper,
    ApplicationDbContext dbContext
) : IReviewRepository
{
    public ListOfReviews GetReviews(int movieId)
    {
        var movieReviews = dbContext
            .Reviews
            .Where(r => r.MovieId == movieId)
            .Include(r => r.Movie)
            .ToList();

        return mapper.Map<ListOfReviews>(movieReviews);
    }

    public ReviewVm GetReview(int movieId, int id)
    {
        var result = dbContext
            .Reviews
            .Where(review => review.Id == id && review.MovieId == movieId)
            .Select(review => new ReviewVm
            (
                review.Id,
                review.Content,
                review.Score,
                review.Movie.Title,
                review.User.Login
            ))
            .FirstOrDefault();

        if (result is null)
        {
            throw new ReviewNotFoundException(id);
        }

        return result;
    }

    public int AddReview(CreateReviewDto reviewDto)
    {
        var review = mapper.Map<Review>(reviewDto);
        dbContext.Add(review);

        dbContext.SaveChanges();

        return review.Id;
    }

    public void UpdateReview(int id, UpdateReviewDto dto)
    {
        var review = GetReviewByIdAndThrowIfNotFound(id);
        review.Content = dto.Content;
        review.Score = dto.Score;

        dbContext.SaveChanges();
    }

    public void DeleteReview(int id)
    {
        var review = GetReviewByIdAndThrowIfNotFound(id);
        dbContext.Remove(review);

        dbContext.SaveChanges();
    }

    private Review GetReviewByIdAndThrowIfNotFound(int id)
    {
        var review = dbContext.Reviews.FirstOrDefault(r => r.Id == id);
        if (review is null)
        {
            throw new ReviewNotFoundException(id);
        }

        return review;
    }
}
