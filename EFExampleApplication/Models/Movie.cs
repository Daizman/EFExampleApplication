namespace EFExampleApplication.Models;

public class Movie
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int DurationInMinutes { get; set; }

    public virtual ICollection<GenreInMovie> Genres { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
}
