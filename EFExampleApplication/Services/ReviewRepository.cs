using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;

namespace EFExampleApplication.Services;

public class ReviewRepository : IReviewRepository
{
    public int AddReview(CreateReviewDto reviewDto)
    {
        throw new NotImplementedException();
    }

    public void DeleteReview(int id)
    {
        throw new NotImplementedException();
    }

    public ReviewVm GetReview(int id)
    {
        throw new NotImplementedException();
    }

    public ListOfReviews GetReviews(int movieId)
    {
        throw new NotImplementedException();
    }

    public void UpdateReview(int id, UpdateReviewDto dto)
    {
        throw new NotImplementedException();
    }
}
