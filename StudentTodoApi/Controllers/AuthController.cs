using Microsoft.AspNetCore.Mvc;
using StudentTodoApi.Models;
using StudentTodoApi.Data;
using System.Linq;

namespace StudentTodoApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (DataStore.Users.Any(u => u.Username == user.Username))
            {
                return BadRequest(new { message = "Username already exists" });
            }

            user.Id = DataStore.GetNextUserId();
            user.Role = "Student"; // All new registrations are Students
            
            DataStore.Users.Add(user);
            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginRequest)
        {
            var user = DataStore.Users.FirstOrDefault(u => 
                u.Username == loginRequest.Username && 
                u.Password == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(new 
            { 
                id = user.Id, 
                username = user.Username, 
                name = user.Name,
                section = user.Section,
                role = user.Role 
            });
        }
    }
}
