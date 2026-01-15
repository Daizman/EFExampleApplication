using EFExampleApplication.Contracts;
using EFExampleApplication.Models;

namespace EFExampleApplication.Abstractions;

public interface IMovieRepository
{
    IReadOnlyList<Movie> GetMovies();
    Movie? GetMovie(int movieId);
    int AddMovie(Movie movie);
    bool UpdateGenresForMovie(int movieId, int[] newGenreIds);
    bool UpdateMovie(
        int movieId,
        string? title,
        string? description,
        int? durationInMinutes
    );
    bool DeleteMovie(int movieId);
}
