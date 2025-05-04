using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class UserRepository(
    IMapper mapper
) : IUserRepository
{
    private readonly List<User> _users = new();

    private readonly IMapper _mapper = mapper;

    public ListOfUsers GetUsers() => _mapper.Map<ListOfUsers>(_users);

    public UserVm GetUserById(int id)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);
        return _mapper.Map<UserVm>(user);
    }

    public UserVm GetUserByLogin(string login)
    {
        var user = _users.FirstOrDefault(user => user.Login == login);
        if (user is null)
        {
            throw new UserNotFoundException(login);
        }

        return _mapper.Map<UserVm>(user);
    }

    public int AddUser(CreateUserDto dto)
    {
        var userId = _users.Count + 1;
        var user = _mapper.Map<User>(dto);
        user.Id = userId;
        _users.Add(user);

        return userId;
    }

    public void UpdateUser(int id, UpdateUserDto dto)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);

        user.Login = dto.Login;
    }

    public void DeleteUser(int id)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);

        _users.Remove(user);
    }

    private User TryGetUserByIdAndThrowIfNotFound(int id)
    {
        var user = _users.FirstOrDefault(user => user.Id == id);
        if (user is null)
        {
            throw new UserNotFoundException(id);
        }

        return user;
    }
}
