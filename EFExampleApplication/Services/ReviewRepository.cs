using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Database;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

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
