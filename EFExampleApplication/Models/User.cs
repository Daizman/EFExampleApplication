namespace EFExampleApplication.Models;

public class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = [];
}
