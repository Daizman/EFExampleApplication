using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Services;

public class UserService(
    IApplicationDbContext applicationDbContext,
    IMapper mapper
) : IUserService
{
    public UserVm GetUserById(UserId id)
    {
        var user = applicationDbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectTo<UserVm>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        if (user is null)
        {
            throw new UserNotFoundException(id);
        }

        return user;
    }

    public UserVm GetUserByLogin(string login)
    {
        var user = applicationDbContext.Users
            .AsNoTracking()
            .Where(u => u.Login == login)
            .ProjectTo<UserVm>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        if (user is null)
        {
            throw new UserNotFoundException(login);
        }

        return user;
    }

    public ListOfUsers GetUsers()
    {
        var users = applicationDbContext.Users
            .AsNoTracking()
            .Select(u => new UserListVm(u.Id, u.Login))
            .ToHashSet();

        return new ListOfUsers(users);
    }

    public int AddUser(CreateUserDto dto)
    {
        var newUser = mapper.Map<User>(dto);

        applicationDbContext.Users.Add(newUser);

        applicationDbContext.SaveChanges();

        return newUser.Id;
    }

    public void UpdateUser(UserId id, UpdateUserDto dto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Id == id);

        if (user is null)
        {
            throw new UserNotFoundException(id);
        }

        user.Login = dto.Login ?? user.Login;
        
        applicationDbContext.SaveChanges();
    }

    public void DeleteUser(UserId id)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Id == id);

        if (user is null)
        {
            throw new UserNotFoundException(id);
        }

        applicationDbContext.Users.Remove(user);

        applicationDbContext.SaveChanges();
    }
}
