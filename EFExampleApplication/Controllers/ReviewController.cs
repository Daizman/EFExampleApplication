using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EFExampleApplication.Controllers;

public class ReviewController(
    IReviewRepository reviewRepository
) : BaseController
{
    [HttpGet("{movieId}")]
    public ActionResult<ListOfReviews> GetReviews(int movieId) => Ok(reviewRepository.GetReviews(movieId));

    [HttpGet("{movieId}/{id}")]
    public ActionResult<ReviewVm> GetReview(int movieId, int id) => Ok(reviewRepository.GetReview(movieId, id));

    [HttpPost]
    public ActionResult<int> AddReview(CreateReviewDto dto)
    {
        var id = reviewRepository.AddReview(dto);

        return CreatedAtAction(nameof(GetReview), new { dto.MovieId, id }, id);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateReview(int id, UpdateReviewDto dto)
    {
        reviewRepository.UpdateReview(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteReview(int id)
    {
        reviewRepository.DeleteReview(id);

        return NoContent();
    }
}
