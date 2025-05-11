using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Database;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class UserRepository(
    IMapper mapper,
    ApplicationDbContext dbContext
) : IUserRepository
{
    private readonly IMapper _mapper = mapper;

    public ListOfUsers GetUsers() => _mapper.Map<ListOfUsers>(dbContext.Users.ToList());

    public UserVm GetUserById(int id)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);

        return _mapper.Map<UserVm>(user);
    }

    public UserVm GetUserByLogin(string login)
    {
        var user = dbContext.Users.FirstOrDefault(user => user.Login == login);

        if (user is null)
        {
            throw new UserNotFoundException(login);
        }

        return _mapper.Map<UserVm>(user);
    }

    public int AddUser(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        dbContext.Add(user);

        dbContext.SaveChanges();

        return user.Id;
    }

    public void UpdateUser(int id, UpdateUserDto dto)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);
        user.Login = dto.Login;

        dbContext.SaveChanges();
    }

    public void DeleteUser(int id)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);
        dbContext.Remove(user);

        dbContext.SaveChanges();
    }

    private User TryGetUserByIdAndThrowIfNotFound(int id)
    {
        var user = dbContext.Users.FirstOrDefault(user => user.Id == id);

        if (user is null)
        {
            throw new UserNotFoundException(id);
        }

        return user;
    }
}
