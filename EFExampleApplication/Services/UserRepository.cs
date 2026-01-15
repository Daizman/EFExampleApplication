using EFExampleApplication.Abstractions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class UserRepository: IUserRepository
{
    private readonly List<User> _users = new();

    public IReadOnlyList<User> GetUsers() => _users;

    public User? GetUserById(int userId)
    {
        return _users.FirstOrDefault(user => user.Id == userId);
    }

    public User? GetUserByLogin(string login)
    {
        return _users.FirstOrDefault(user => user.Login == login);
    }

    public int AddUser(User newUser)
    {
        var userId = _users.Count + 1;
        newUser.Id = userId;
        _users.Add(newUser);

        return userId;
    }

    public bool UpdateUser(int userId, string login)
    {
        var user = _users.FirstOrDefault(user => user.Id == userId);

        if (user is null)
        {
            return false;
        }

        user.Login = login;
        return true;
    }

    public bool DeleteUser(int userId)
    {
        var user = _users.FirstOrDefault(user => user.Id == userId);

        if (user is null)
        {
            return false;
        }

        _users.Remove(user);
        return true;
    }
}
