using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Services;

public class ReviewService(
    IApplicationDbContext applicationDbContext,
    IMapper mapper
) : IReviewService
{
    public int AddReview(CreateReviewDto reviewDto)
    {
        var movieExists = applicationDbContext.Movies.Any(m => m.Id == reviewDto.MovieId);

        if (!movieExists)
        {
            throw new MovieNotFoundException(reviewDto.MovieId);
        }

        var userExists = applicationDbContext.Users.Any(u => u.Id == reviewDto.UserId);

        if (!userExists)
        {
            throw new UserNotFoundException(reviewDto.UserId);
        }

        var review = mapper.Map<Review>(reviewDto);

        applicationDbContext.Reviews.Add(review);

        applicationDbContext.SaveChanges();

        return review.Id;
    }

    public void DeleteReview(int id)
    {
        var deleted = applicationDbContext.Reviews
            .Where(r => r.Id == id)
            .ExecuteDelete();
        if (deleted == 0)
        {
            throw new ReviewNotFoundException(id);
        }
    }

    public ReviewVm GetReview(int movieId, int id)
    {
        var review = applicationDbContext.Reviews
            .Include(r => r.Movie)
            .Include(r => r.User)
            .AsNoTracking()
            .FirstOrDefault(r => r.MovieId == movieId && r.Id == id);
        if (review is null)
        {
            throw new ReviewNotFoundException(movieId, id);
        }

        return mapper.Map<ReviewVm>(review);
    }

    public ListOfReviews GetReviews(int movieId)
    {
        var movie = applicationDbContext.Movies
            .Include(r => r.Reviews)
            .AsNoTracking()
            .FirstOrDefault(m => m.Id == movieId);

        return mapper.Map<ListOfReviews>(movie);
    }

    public void UpdateReview(int id, UpdateReviewDto dto)
    {
        var updated = applicationDbContext.Reviews
            .Where(r => r.Id == id)
            .ExecuteUpdate(setters => setters
                .SetProperty(r => r.Content, r => dto.Content ?? r.Content)
                .SetProperty(r => r.Score, r => dto.Score ?? r.Score)
            );

        if (updated == 0)
        {
            throw new ReviewNotFoundException(id);
        }
    }
}
