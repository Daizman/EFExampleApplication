namespace EFExampleApplication.Models;

public class Movie
{
    public int Id { get; set; }
    // required означает, что это поле обязательно для заполнения
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int DurationInMinutes { get; set; }
}
