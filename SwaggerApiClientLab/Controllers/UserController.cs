using Microsoft.AspNetCore.Mvc;

namespace SwaggerApiClientLab.Controllers;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    public ActionResult<User> GetUser(int id)
    {
        var user = new User
        {
            Id = id,
            Name = $"User {id}"
        };
        
        return Ok(user);
    }
}