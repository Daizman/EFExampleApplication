using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Services;

public class UserService(
    // Заменили IUserRepository на IAppliactionDbContext
    IApplicationDbContext applicationDbContext,
    IMapper mapper
) : IUserService
{
    public UserVm GetUserById(int id)
    {
        // Теперь достаем из контекста, а не из репозитория.
        // AsNoTracking используется в GET запросах для оптимизации.
        var user = applicationDbContext.Users.AsNoTracking().FirstOrDefault(user => user.Id == id);

        // Выносить как-то отдельно в метод TryGetUserAndThrowIfNotFound не особо имеет смысл,
        // так как в этом случае нам все равно придется подглядывать в его реализацию, чтобы
        // узнать, используется там AsNoTracking или нет.
        // Т.е. абстракция не будет работать.
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

        // Теперь при изменении мы явно вызываем сохранение в БД.
        // На самом деле, это основной момент, который нам мешает нормально работать из репозиториев
        // с контекстом. Скорее всего, все методы репозитория нацеленные на изменение данных в конце
        // будут иметь вызов SaveChanges(), что приведет к тому, что мы не сможем объединять операции
        // изменения в атомарные цепочки.
        applicationDbContext.SaveChanges();

        return newUser.Id;
    }

    public void UpdateUser(int id, UpdateUserDto dto)
    {
        // Так как мы собираемся изменять полученного пользователя, нам нужно отслеживать то,
        // что происходит с объектом класса, поэтому тут НЕТ AsNoTracking
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Id == id);

        if (user is null)
        {
            throw new UserNotFoundException(id);
        }

        // Вполне стандартный подход, когда мы обновляем всегда, просто в случае, если null
        // используем старое значение
        user.Login = dto.Login ?? user.Login;

        // Не забываем про вызов SaveChanges()
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
