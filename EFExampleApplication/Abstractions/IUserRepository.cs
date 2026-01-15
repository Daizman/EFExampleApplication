using EFExampleApplication.Models;

namespace EFExampleApplication.Abstractions;

public interface IUserRepository
{
    IReadOnlyList<User> GetUsers();
    User? GetUserById(int userId);
    User? GetUserByLogin(string login);
    int AddUser(User newUser);
    bool UpdateUser(int userId, string login);
    bool DeleteUser(int userId);
}
