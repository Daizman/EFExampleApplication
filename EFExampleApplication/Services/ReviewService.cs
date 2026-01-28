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
    IMapper mapper
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
            throw new MovieNotFoundException(movieId);
        }

        return movieReviews;
    }

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

    public void DeleteReview(ReviewId id)
    {
        var deleted = applicationDbContext.Reviews
            .Where(r => r.Id == id)
            .ExecuteDelete();
        if (deleted == 0)
        {
            throw new ReviewNotFoundException(id);
        }
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
            throw new ReviewNotFoundException(id);
        }
    }
}
