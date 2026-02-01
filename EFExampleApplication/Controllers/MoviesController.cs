using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EFExampleApplication.Controllers;

public class MoviesController(
    IMovieService movieService
) : BaseController
{
    [HttpGet]
    public ActionResult<ListOfMovies> GetMovies() => Ok(movieService.GetMovies());

    [HttpGet("{id}")]
    public ActionResult<MovieVm> GetMovie(int id) => Ok(movieService.GetMovie(id));

    [Obsolete("Use POST /api/MoviesV2 instead")]
    [HttpPost]
    public ActionResult<int> AddMovie(CreateMovieDto dto)
    {
        var id = movieService.AddMovie(dto);

        return CreatedAtAction(nameof(GetMovie), new { id }, id);
    }

    [Obsolete("Use PUT /api/MoviesV2/{id} instead")]
    [HttpPut("{id}/genres")]
    public ActionResult UpdateGenresForMovie(int id, UpdateGenresForMovieDto dto)
    {
        movieService.UpdateGenresForMovie(id, dto);

        return NoContent();
    }

    [Obsolete("Use PUT /api/MoviesV2/{id} instead")]
    [HttpPut("{id}")]
    public ActionResult UpdateMovie(int id, UpdateMovieDto dto)
    {
        movieService.UpdateMovie(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteMovie(int id)
    {
        movieService.DeleteMovie(id);

        return NoContent();
    }
}
