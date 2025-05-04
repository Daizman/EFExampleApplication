using EFExampleApplication.Contracts;

namespace EFExampleApplication.Abstractions;

public interface IMovieRepository
{
    ListOfMovies GetMovies();
    MovieVm GetMovie(int id);
    int AddMovie(CreateMovieDto movieDto);
    void UpdateGenresForMovie(int id, UpdateGenresForMovieDto dto);
    void UpdateMovie(int id, UpdateMovieDto dto);
    void DeleteMovie(int id);
}
