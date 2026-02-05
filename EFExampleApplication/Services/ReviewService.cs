using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Services;

public class ReviewService(
    IApplicationDbContext applicationDbContext,
    IMapper mapper,
    ILogger<ReviewService> logger
) : IReviewService
{
    public ReviewVm GetReview(MovieId movieId, ReviewId id)
    {
        var review = applicationDbContext.Reviews
            .AsNoTracking()
            .Where(r => r.MovieId == movieId && r.Id == id)
            .ProjectTo<ReviewVm>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        if (review is null)
        {
            logger.LogError("Review with {Id} for movie {MovieId} not found.", id, movieId);
            throw new ReviewNotFoundException(movieId, id);
        }

        return review;
    }

    public ListOfReviews GetReviews(MovieId movieId)
    {
        var movieReviews = applicationDbContext.Movies
            .AsNoTracking()
            .Where(m => m.Id == movieId)
            .Include(r => r.Reviews)
            .ProjectTo<ListOfReviews>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        if (movieReviews is null)
        {
            logger.LogError("Reviews for movie {MovieId} not found.", movieId);
            throw new MovieNotFoundException(movieId);
        }

        return movieReviews;
    }

    public int AddReview(CreateReviewDto reviewDto)
    {
        var movieExists = applicationDbContext.Movies.Any(m => m.Id == reviewDto.MovieId);

        if (!movieExists)
        {
            logger.LogError("[{UserId}] Couldn't add review to movie {MovieId}. Movie not found", reviewDto.UserId, reviewDto.MovieId);
            throw new MovieNotFoundException(reviewDto.MovieId);
        }

        var userExists = applicationDbContext.Users.Any(u => u.Id == reviewDto.UserId);

        if (!userExists)
        {
            logger.LogError("[{UserId}] Couldn't add review to movie {MovieId}. User not found", reviewDto.UserId, reviewDto.MovieId);
            throw new UserNotFoundException(reviewDto.UserId);
        }

        var review = mapper.Map<Review>(reviewDto);

        applicationDbContext.Reviews.Add(review);

        applicationDbContext.SaveChanges();

        logger.LogInformation("[{UserId}] Successfully added new review for movie {MovieId}", reviewDto.UserId, reviewDto.MovieId);

        return review.Id;
    }

    public void DeleteReview(ReviewId id)
    {
        var deleted = applicationDbContext.Reviews
            .Where(r => r.Id == id)
            .ExecuteDelete();
        if (deleted == 0)
        {
            logger.LogError("Coudln't delete review with {Id}. Not found.", id);
            throw new ReviewNotFoundException(id);
        }

        logger.LogInformation("Successfully deleted review {Id}", id);
    }

    public void UpdateReview(ReviewId id, UpdateReviewDto dto)
    {
        var updated = applicationDbContext.Reviews
            .Where(r => r.Id == id)
            .ExecuteUpdate(setters => setters
                .SetProperty(r => r.Content, r => dto.Content ?? r.Content)
                .SetProperty(r => r.Score, r => dto.Score ?? r.Score)
            );

        if (updated == 0)
        {
            logger.LogError("Coudln't update review with {Id}. Not found.", id);
            throw new ReviewNotFoundException(id);
        }

        logger.LogInformation("Successfully updated review {Id}", id);
    }
}
