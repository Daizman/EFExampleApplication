using EFExampleApplication.Contracts;

namespace EFExampleApplication.Abstractions;

public interface IUserRepository
{
    ListOfUsers GetUsers();
    UserVm GetUserById(int id);
    UserVm GetUserByLogin(string login);
    int AddUser(CreateUserDto dto);
    void UpdateUser(int id, UpdateUserDto dto);
    void DeleteUser(int id);
}
