using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class ReviewRepository(
    IMapper mapper,
    IUserRepository userRepository,
    IMovieRepository movieRepository
) : IReviewRepository
{
    private readonly List<Review> _reviews = new();

    public ListOfReviews GetReviews(int movieId)
    {
        var movie = movieRepository.GetMovie(movieId);
        var movieReviews = _reviews.Where(r => r.MovieId == movieId);

        return mapper.Map<ListOfReviews>((movie, movieReviews));
    }

    public ReviewVm GetReview(int movieId, int id)
    {
        var movie = movieRepository.GetMovie(movieId);
        var review = GetReviewByIdAndThrowIfNotFound(id);
        var user = userRepository.GetUserById(review.UserId);

        return mapper.Map<ReviewVm>((movie, user, review));
    }

    public int AddReview(CreateReviewDto reviewDto)
    {
        var movie = movieRepository.GetMovie(reviewDto.MovieId);
        var user = userRepository.GetUserById(reviewDto.UserId);
        var review = mapper.Map<Review>(reviewDto);
        review.Id = _reviews.Count + 1;
        review.MovieId = movie.Id;
        review.UserId = user.Id;

        _reviews.Add(review);

        return review.Id;
    }

    public void UpdateReview(int id, UpdateReviewDto dto)
    {
        var review = GetReviewByIdAndThrowIfNotFound(id);

        review.Content = dto.Content;
        review.Score = dto.Score;
    }

    public void DeleteReview(int id)
    {
        var review = GetReviewByIdAndThrowIfNotFound(id);
        _reviews.Remove(review);
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
