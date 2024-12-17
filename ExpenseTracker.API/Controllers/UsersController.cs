using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;

using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(UserService _userService, ILogger<UsersController> _logger) : ControllerBase
{
    [HttpPost("update/{userId}")]
    public async Task<ActionResult<User>> Update(UserDTO user, int userId)
    {
        User? _user = await _userService.GetUserById(userId);

        if (user == null)
        {
            return BadRequest("Wrong user identifier");
        }

        if (_user?.Id != userId)
        {
            return BadRequest("No match found");
        }

        _user.Email = user.Email;
        _user.FirstName = user.FirstName;
        _user.LastName = user.LastName;
        _user.PasswordHash = user.Password;
        _user.Username = user.Username;

        var res = await _userService.UpdateUserAsync(_user);

        return Ok(res);
    }

    [HttpGet("all")]
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return (await _userService.GetAllUsersAsync()).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userService.GetUserById(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpDelete("remove/{userId}")]
    public async Task<ActionResult<bool>> RemoveUser(int userId)
    {
        var existingUser = await _userService.GetUserById(userId);
        if (existingUser == null)
        {
            return BadRequest("No user found");
        }

        var res = await _userService.RemoveUser(userId);

        if (!res)
        {
            return BadRequest("Failed to remove user");
        }

        return Ok(res);
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> CreateUser(UserDTO user)
    {
        if (user == null)
        {
            return BadRequest("No user data provided");
        }

        var newUser = new User
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PasswordHash = user.Password,
            Username = user.Username
        };

        var res = await _userService.CreateUserAsync(newUser);

        return Ok(res);
    }


}