using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Database;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Services;

public class UserRepository(
    IMapper mapper,
    // Внедряем контекст базы данных, чтобы использовать его в репозитории
    ApplicationDbContext dbContext
) : IUserRepository
{
    private readonly IMapper _mapper = mapper;

    // Ходим в базу данных, чтобы получить пользователей
    public ListOfUsers GetUsers() => _mapper.Map<ListOfUsers>(dbContext.Users.ToList());

    public UserVm GetUserById(int id)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);
        return _mapper.Map<UserVm>(user);
    }

    public UserVm GetUserByLogin(string login)
    {
        // Ищем пользователя по логину в списке пользователей в БД через контекст
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

        ExecuteWithSave(() => dbContext.Add(user));

        return user.Id;
    }

    public void UpdateUser(int id, UpdateUserDto dto) => ExecuteWithSave(() =>
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);

        user.Login = dto.Login;
    });

    public void DeleteUser(int id) => ExecuteWithSave(() =>
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);

        dbContext.Remove(user);
    });

    private User TryGetUserByIdAndThrowIfNotFound(int id)
    {
        // Ищем пользователя по id в списке пользователей в БД через контекст
        var user = dbContext.Users.FirstOrDefault(user => user.Id == id);
        if (user is null)
        {
            throw new UserNotFoundException(id);
        }

        return user;
    }

    private void ExecuteWithSave(Action action)
    {
        try
        {
            action();
            dbContext.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            throw new DbUpdateException("Database update error occurred", ex);
        }
    }
}
