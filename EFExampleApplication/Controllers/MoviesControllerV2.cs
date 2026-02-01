using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts.V2;
using Microsoft.AspNetCore.Mvc;

namespace EFExampleApplication.Controllers;

public class MoviesV2Controller(IMovieService movieService) : BaseController
{
    [HttpPost]
    public ActionResult<int> AddMovie(CreateMovieV2Dto dto)
    {
        var id = movieService.AddMovie(dto);

        return CreatedAtAction(nameof(MoviesController.GetMovie), "Movies", new { id }, id);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateMovie(int id, UpdateMovieV2Dto dto)
    {
        movieService.UpdateMovie(id, dto);

        return NoContent();
    }
}
