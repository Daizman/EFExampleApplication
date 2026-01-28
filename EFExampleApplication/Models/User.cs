namespace EFExampleApplication.Models;

public class User
{
    public UserId Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }

    public ICollection<Review> Reviews { get; set; } = [];
}
