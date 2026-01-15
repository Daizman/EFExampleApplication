using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class UserService(
    IUserRepository userRepository,
    IMapper mapper
) : IUserService
{
    public int AddUser(CreateUserDto dto)
    {
        var newUser = mapper.Map<User>(dto);
        return userRepository.AddUser(newUser);
    }

    public void DeleteUser(int id)
    {
        var _ = userRepository.DeleteUser(id);
    }

    public UserVm GetUserById(int id)
    {
        var user = userRepository.GetUserById(id);

        if (user is null)
        {
            throw new UserNotFoundException(id);
        }

        return mapper.Map<UserVm>(user);
    }

    public UserVm GetUserByLogin(string login)
    {
        var user = userRepository.GetUserByLogin(login);

        if (user is null)
        {
            throw new UserNotFoundException(login);
        }

        return mapper.Map<UserVm>(user);
    }

    public ListOfUsers GetUsers()
    {
        var users = userRepository.GetUsers();

        return mapper.Map<ListOfUsers>(users);
    }

    public void UpdateUser(int id, UpdateUserDto dto)
    {
        var _ = userRepository.UpdateUser(id, dto.Login);
    }
}
