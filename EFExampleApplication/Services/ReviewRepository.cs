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
    ApplicationDbContext dbContext,
    IUserRepository userRepository,
    IMovieRepository movieRepository
) : IReviewRepository
{
    public ListOfReviews GetReviews(int movieId)
    {
        var movie = movieRepository.GetMovie(movieId);
        var movieReviews = dbContext.Reviews.Where(r => r.MovieId == movieId).ToList();

        return mapper.Map<ListOfReviews>((movie, movieReviews));
    }

    public ReviewVm GetReview(int movieId, int id)
    {
        var review = GetReviewByIdAndThrowIfNotFound(id);
        var movie = movieRepository.GetMovie(movieId);
        var user = userRepository.GetUserById(review.UserId);

        return mapper.Map<ReviewVm>((movie, user, review));
    }

    public int AddReview(CreateReviewDto reviewDto)
    {
        var movie = movieRepository.GetMovie(reviewDto.MovieId);
        var user = userRepository.GetUserById(reviewDto.UserId);
        var review = mapper.Map<Review>(reviewDto);
        review.MovieId = movie.Id;
        review.UserId = user.Id;

        ExecuteWithSave(() => dbContext.Add(review));

        return review.Id;
    }

    public void UpdateReview(int id, UpdateReviewDto dto) => ExecuteWithSave(() =>
    {
        var review = GetReviewByIdAndThrowIfNotFound(id);

        review.Content = dto.Content;
        review.Score = dto.Score;
    });

    public void DeleteReview(int id) => ExecuteWithSave(() =>
    {
        var review = GetReviewByIdAndThrowIfNotFound(id);

        dbContext.Remove(review);
    });

    private Review GetReviewByIdAndThrowIfNotFound(int id)
    {
        var review = dbContext.Reviews.FirstOrDefault(r => r.Id == id);
        if (review is null)
        {
            throw new ReviewNotFoundException(id);
        }

        return review;
    }

    private void ExecuteWithSave(Action action)
    {
        try
        {
            action.Invoke();
            dbContext.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            throw new DbUpdateException("Database update error occurred", ex);
        }
    }
}
