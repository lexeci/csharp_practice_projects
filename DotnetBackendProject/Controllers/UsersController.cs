using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetUsers() => Ok(_userService.GetAllUsers());

    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        if (!_userService.ValidateUser(user)) return BadRequest("Invalid user data.");
        _userService.AddUser(user);
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User user)
    {
        if (!_userService.ValidateUser(user)) return BadRequest("Invalid data.");
        if (!_userService.UpdateUser(id, user)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        if (!_userService.DeleteUser(id)) return NotFound();
        return NoContent();
    }
}
