namespace EFExampleApplication.Exceptions;

public class MovieNotFoundException(int id) : Exception($"Movie with id {id} not found.");
