using EFExampleApplication.Contracts;

namespace EFExampleApplication.Abstractions;

public interface IReviewRepository
{
    ListOfReviews GetReviews(int movieId);
    ReviewVm GetReview(int movieId, int id);
    int AddReview(CreateReviewDto reviewDto);
    void UpdateReview(int id, UpdateReviewDto dto);
    void DeleteReview(int id);
}
