using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EFExampleApplication.Controllers;

public class MovieController(
    IMovieRepository movieRepository
) : BaseController
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    [HttpGet]
    public ActionResult<ListOfMovies> GetMovies() => Ok(_movieRepository.GetMovies());

    [HttpGet("{id}")]
    public ActionResult<MovieVm> GetMovie(int id) => Ok(_movieRepository.GetMovie(id));

    [HttpPost]
    public ActionResult<int> AddMovie(CreateMovieDto dto)
    {
        var id = _movieRepository.AddMovie(dto);

        return CreatedAtAction(nameof(GetMovie), new { id }, id);
    }

    [HttpPut("{id}/genres")]
    public ActionResult UpdateGenresForMovie(int id, UpdateGenresForMovieDto dto)
    {
        _movieRepository.UpdateGenresForMovie(id, dto);

        return NoContent();
    }

    [HttpPut("{id}")]
    public ActionResult UpdateMovie(int id, UpdateMovieDto dto)
    {
        _movieRepository.UpdateMovie(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteMovie(int id)
    {
        _movieRepository.DeleteMovie(id);

        return NoContent();
    }
}
