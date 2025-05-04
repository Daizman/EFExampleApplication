using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EFExampleApplication.Controllers;

public class MovieController(
    IMovieRepository movieRepository
) : BaseController
{
    [HttpGet]
    public ActionResult<ListOfMovies> GetMovies() => Ok(movieRepository.GetMovies());

    [HttpGet("{id}")]
    public ActionResult<MovieVm> GetMovie(int id) => Ok(movieRepository.GetMovie(id));

    [HttpPost]
    public ActionResult<int> AddMovie(CreateMovieDto dto)
    {
        var id = movieRepository.AddMovie(dto);

        return CreatedAtAction(nameof(GetMovie), new { id }, id);
    }

    [HttpPut("{id}/genres")]
    public ActionResult UpdateGenresForMovie(int id, UpdateGenresForMovieDto dto)
    {
        movieRepository.UpdateGenresForMovie(id, dto);

        return NoContent();
    }

    [HttpPut("{id}")]
    public ActionResult UpdateMovie(int id, UpdateMovieDto dto)
    {
        movieRepository.UpdateMovie(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteMovie(int id)
    {
        movieRepository.DeleteMovie(id);

        return NoContent();
    }
}
