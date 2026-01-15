using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;

    public MovieService(IMovieRepository movieRepository, IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public int AddMovie(CreateMovieDto movieDto)
    {
        var newMovie = _mapper.Map<Movie>(movieDto);
        var movieId = _movieRepository.AddMovie(newMovie);

        return movieId;
    }

    public void DeleteMovie(int id)
    {
        _ = _movieRepository.DeleteMovie(id);
    }

    public MovieVm GetMovie(int id)
    {
        var movie = _movieRepository.GetMovie(id);

        if (movie is null)
        {
            throw new MovieNotFoundException(id);
        }

        return _mapper.Map<MovieVm>(movie);
    }

    public ListOfMovies GetMovies()
    {
        var movies = _movieRepository.GetMovies();

        return _mapper.Map<ListOfMovies>(movies);
    }

    public void UpdateGenresForMovie(int id, UpdateGenresForMovieDto dto)
    {
        _ = _movieRepository.UpdateGenresForMovie(id, dto.GenreIds);
    }

    public void UpdateMovie(int id, UpdateMovieDto dto)
    {
        _ = _movieRepository.UpdateMovie(id, dto.Title, dto.Description, dto.DurationInMinutes);
    }
}
