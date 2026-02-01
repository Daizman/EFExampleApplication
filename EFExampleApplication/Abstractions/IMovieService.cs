using EFExampleApplication.Contracts;
using EFExampleApplication.Contracts.V2;

namespace EFExampleApplication.Abstractions;

public interface IMovieService
{
    ListOfMovies GetMovies();
    MovieVm GetMovie(int id);
    int AddMovie(CreateMovieDto dto);
    int AddMovie(CreateMovieV2Dto dto);
    void UpdateGenresForMovie(int id, UpdateGenresForMovieDto dto);
    void UpdateMovie(int id, UpdateMovieDto dto);
    void UpdateMovie(int id, UpdateMovieV2Dto dto);
    void DeleteMovie(int id);
}
