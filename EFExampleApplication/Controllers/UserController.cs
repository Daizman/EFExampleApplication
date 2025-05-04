using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EFExampleApplication.Controllers;

public class UserController(
    IUserRepository userRepository
) : BaseController
{
    private readonly IUserRepository _userRepository = userRepository;

    [HttpGet]
    public ActionResult<ListOfUsers> GetUsers()
        => Ok(_userRepository.GetUsers());

    [HttpGet("by_login")]
    public ActionResult<UserVm> GetUser(string login)
    {
        var user = _userRepository.GetUserByLogin(login);

        if (user is null)
        {
            return NotFound(login);
        }

        return Ok(user);
    }

    [HttpPost]
    public ActionResult<int> AddUser(CreateUserDto dto)
        => Ok(_userRepository.AddUser(dto));

    [HttpPut("{id}")]
    public ActionResult UpdateUser(int id, UpdateUserDto dto)
    {
        _userRepository.UpdateUser(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        _userRepository.DeleteUser(id);

        return NoContent();
    }
}
