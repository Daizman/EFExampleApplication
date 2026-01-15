using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EFExampleApplication.Controllers;

public class ReviewController(
    IReviewService reviewService
) : BaseController
{
    [HttpGet("{movieId}")]
    public ActionResult<ListOfReviews> GetReviews(int movieId) => Ok(reviewService.GetReviews(movieId));

    [HttpGet("{movieId}/{id}")]
    public ActionResult<ReviewVm> GetReview(int movieId, int id) => Ok(reviewService.GetReview(movieId, id));

    [HttpPost]
    public ActionResult<int> AddReview(CreateReviewDto dto)
    {
        var id = reviewService.AddReview(dto);

        return CreatedAtAction(nameof(GetReview), new { dto.MovieId, id }, id);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateReview(int id, UpdateReviewDto dto)
    {
        reviewService.UpdateReview(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteReview(int id)
    {
        reviewService.DeleteReview(id);

        return NoContent();
    }
}
