using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EFExampleApplication.Controllers;

public class UsersController(
    IUserService userService
) : BaseController
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    public ActionResult<ListOfUsers> GetUsers()
        => Ok(_userService.GetUsers());

    [HttpGet("by_login")]
    public ActionResult<UserVm> GetUser(string login)
    {
        var user = _userService.GetUserByLogin(login);

        if (user is null)
        {
            return NotFound(login);
        }

        return Ok(user);
    }

    [HttpPost]
    public ActionResult<int> AddUser(CreateUserDto dto)
        => Ok(_userService.AddUser(dto));

    [HttpPut("{id}")]
    public ActionResult UpdateUser(int id, UpdateUserDto dto)
    {
        _userService.UpdateUser(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        _userService.DeleteUser(id);

        return NoContent();
    }
}
