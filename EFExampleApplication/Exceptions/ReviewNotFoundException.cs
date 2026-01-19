namespace EFExampleApplication.Exceptions;

public class ReviewNotFoundException : Exception
{
    public ReviewNotFoundException(int id) : base($"Review with id {id} not found.") { }

    public ReviewNotFoundException(int movieId, int id) : base($"Review with id {id} for movie {movieId} not found.") { }
}
