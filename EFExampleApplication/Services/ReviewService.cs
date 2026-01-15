using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class ReviewService(
    IReviewRepository reviewRepository,
    IMapper mapper
) : IReviewService
{
    public int AddReview(CreateReviewDto reviewDto)
    {
        var review = mapper.Map<Review>(reviewDto);

        return reviewRepository.AddReview(review);
    }

    public void DeleteReview(int id)
    {
        _ = reviewRepository.DeleteReview(id);
    }

    public ReviewVm GetReview(int movieId, int id)
    {
        var review = reviewRepository.GetReview(movieId, id);
        if (review is null)
        {
            throw new ReviewNotFoundException(id);
        }

        return mapper.Map<ReviewVm>(review);
    }

    public ListOfReviews GetReviews(int movieId)
    {
        var reviews = reviewRepository.GetReviews(movieId);

        return mapper.Map<ListOfReviews>(reviews);
    }

    public void UpdateReview(int id, UpdateReviewDto dto)
    {
        reviewRepository.UpdateReview(id, dto.Content, dto.Score);
    }
}
