using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private static readonly Dictionary<int, User> users = new()
    {
        { 1, new User { Id = 1, Name = "Alice", Age = 21 } },
        { 2, new User { Id = 2, Name = "Bob", Age = 22 } }
    };

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(users.Values);
    }

    [HttpGet("{id:int:min(1)}")]
    public IActionResult GetUser(int id)
    {
        return users.TryGetValue(id, out var user) ? Ok(user) : NotFound();
    }

    [HttpPost]
    public IActionResult PostUser([FromBody] User newUser)
    {   
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int newId = users.Keys.Any() ? users.Keys.Max() + 1 : 1;
        newUser.Id = newId;
        users[newId] = newUser;
        return CreatedAtAction(nameof(GetUser), new { id = newId }, newUser); // Returning 201 Created status
    }

    [HttpPut("{id:int:min(1)}")]
    public IActionResult PutUser(int id, [FromBody] User updatedUser)
    {
        if (!users.ContainsKey(id))
        {
            return NotFound($"User with id:{id} not found");
        }

        if (updatedUser == null || string.IsNullOrEmpty(updatedUser.Name) || updatedUser.Age < 1)
        {
            return BadRequest("Invalid user data");
        }

        updatedUser.Id = id;
        users[id] = updatedUser;
        return Ok(updatedUser);
    }

    [HttpDelete("{id:int:min(1)}")]
    public IActionResult DeleteUser(int id)
    {
        if (!users.Remove(id))
        {
            return NotFound(new { message = $"User with id:{id} not found" });
        }

        return Ok(new { message = $"User with id:{id} has been deleted" });
    }
}

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; }

    [Range(1, 150)]
    public int Age { get; set; }
}
