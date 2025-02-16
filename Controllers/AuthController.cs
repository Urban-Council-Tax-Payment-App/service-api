using System.Text;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using service_api.DTOs;
using service_api.Models;
using service_api.Services;
using System.Security.Cryptography;
namespace service_api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class AuthController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;

        public AuthController(MongoDbService mongoDbService)
        {
            _users = mongoDbService.GetCollection<User>("Users");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest("Email is already registered.");
            }

            var hashedPassword = HashPassword(dto.Password);

            var user = new User
            {
                Name = dto.Name,
                NIC = dto.NIC,
                Email = dto.Email,
                PasswordHash = hashedPassword
            };

            await _users.InsertOneAsync(user);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok("Login successful.");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
