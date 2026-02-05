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
    IMapper mapper,
    ILogger<UserService> logger
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
            logger.LogError("User with {Id} not found", id);
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
            logger.LogError("User with {Login} not found", login);
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

        logger.LogInformation("User successfully added {Id}", newUser.Id);

        return newUser.Id;
    }

    public void UpdateUser(UserId id, UpdateUserDto dto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Id == id);

        if (user is null)
        {
            logger.LogError("Coudn't update user with {Id}. Not found", id);
            throw new UserNotFoundException(id);
        }

        user.Login = dto.Login;
        
        applicationDbContext.SaveChanges();

        logger.LogInformation("User successfully updated {Id}", id);
    }

    public void DeleteUser(UserId id)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Id == id);

        if (user is null)
        {
            logger.LogError("Coudn't delete user with {Id}. Not found", id);
            throw new UserNotFoundException(id);
        }

        applicationDbContext.Users.Remove(user);

        applicationDbContext.SaveChanges();

        logger.LogInformation("User successfully deleted {Id}", id);
    }
}
