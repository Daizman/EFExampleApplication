namespace EFExampleApplication.Models;

public class Review
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public int Score { get; set; }

    public int MovieId { get; set; }
    public virtual required Movie Movie { get; set; }

    public int UserId { get; set; }
    public virtual required User User { get; set; }
}
