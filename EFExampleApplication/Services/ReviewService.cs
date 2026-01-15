using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class ReviewService(
    IReviewRepository reviewRepository,
    IMovieRepository movieRepository,
    IUserRepository userRepository,
    IMapper mapper
) : IReviewService
{
    public int AddReview(CreateReviewDto reviewDto)
    {
        var movie = GetMoviewAndThrowIfNotFound(reviewDto.MovieId);
        var user = GetUserAndThrowIfNotFound(reviewDto.UserId);
        var review = mapper.Map<Review>(reviewDto);
        review.MovieId = movie.Id;
        review.UserId = user.Id;

        return reviewRepository.AddReview(review);
    }

    public void DeleteReview(int id)
    {
        _ = reviewRepository.DeleteReview(id);
    }

    public ReviewVm GetReview(int movieId, int id)
    {
        var movie = GetMoviewAndThrowIfNotFound(movieId);

        var review = reviewRepository.GetReview(movieId, id);
        if (review is null)
        {
            throw new ReviewNotFoundException(id);
        }

        var user = GetUserAndThrowIfNotFound(review.UserId);

        return mapper.Map<ReviewVm>((movie, user, review));
    }

    public ListOfReviews GetReviews(int movieId)
    {
        var movie = movieRepository.GetMovie(movieId);
        var reviews = reviewRepository.GetReviews(movieId);

        return mapper.Map<ListOfReviews>((movie, reviews));
    }

    public void UpdateReview(int id, UpdateReviewDto dto)
    {
        reviewRepository.UpdateReview(id, dto.Content, dto.Score);
    }

    private Movie GetMoviewAndThrowIfNotFound(int movieId)
    {
        var movie = movieRepository.GetMovie(movieId);
        if (movie is null)
        {
            throw new MovieNotFoundException(movieId);
        }

        return movie;
    }

    private User GetUserAndThrowIfNotFound(int userId)
    {
        var user = userRepository.GetUserById(userId);

        return user ?? throw new UserNotFoundException(userId);
    }
}
