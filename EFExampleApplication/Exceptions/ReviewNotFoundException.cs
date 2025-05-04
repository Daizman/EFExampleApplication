namespace EFExampleApplication.Exceptions;

public class ReviewNotFoundException(int id) : Exception($"Review with id {id} not found.");
