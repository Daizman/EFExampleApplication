using AutoMapper;
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
    public UserVm GetUserById(int id)
    {
        var user = applicationDbContext.Users.AsNoTracking().FirstOrDefault(user => user.Id == id);

        if (user is null)
        {
            throw new UserNotFoundException(id);
        }

        return mapper.Map<UserVm>(user);
    }

    public UserVm GetUserByLogin(string login)
    {
        var user = applicationDbContext.Users.AsNoTracking().FirstOrDefault(user => user.Login == login);

        if (user is null)
        {
            throw new UserNotFoundException(login);
        }

        return mapper.Map<UserVm>(user);
    }

    public ListOfUsers GetUsers()
    {
        var users = applicationDbContext.Users.AsNoTracking().ToList();

        return mapper.Map<ListOfUsers>(users);
    }

    public int AddUser(CreateUserDto dto)
    {
        var newUser = mapper.Map<User>(dto);

        applicationDbContext.Users.Add(newUser);

        applicationDbContext.SaveChanges();

        return newUser.Id;
    }

    public void UpdateUser(int id, UpdateUserDto dto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Id == id);

        if (user is null)
        {
            throw new UserNotFoundException(id);
        }

        user.Login = dto.Login ?? user.Login;
        
        applicationDbContext.SaveChanges();
    }

    public void DeleteUser(int id)
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
