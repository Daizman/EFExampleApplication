namespace EFExampleApplication.Exceptions;

public class GenreNotFoundException(int id) : Exception($"Genre with id {id} not found.");
