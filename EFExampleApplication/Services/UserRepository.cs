using EFExampleApplication.Abstractions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Services;

public class UserRepository(IApplicationDbContext applicationDbContext): IUserRepository
{
    private readonly DbSet<User> _users = applicationDbContext.Users;

    public IReadOnlyList<User> GetUsers() => _users.AsNoTracking().ToList();

    public User? GetUserById(int userId)
    {
        return _users.AsNoTracking().FirstOrDefault(user => user.Id == userId);
    }

    public User? GetUserByLogin(string login)
    {
        return _users.AsNoTracking().FirstOrDefault(user => user.Login == login);
    }

    public int AddUser(User newUser)
    {
        _users.Add(newUser);

        applicationDbContext.SaveChanges();

        return newUser.Id;
    }

    public bool UpdateUser(int userId, string login)
    {
        var user = _users.FirstOrDefault(user => user.Id == userId);

        if (user is null)
        {
            return false;
        }

        user.Login = login;

        applicationDbContext.SaveChanges();

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

        applicationDbContext.SaveChanges();

        return true;
    }
}
