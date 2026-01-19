namespace EFExampleApplication.Exceptions;

public class GenreNotFoundException : Exception
{
    public GenreNotFoundException(int id) : base($"Genre with id {id} not found.") { }

    public GenreNotFoundException() : base("Genre not found.") { }
}
