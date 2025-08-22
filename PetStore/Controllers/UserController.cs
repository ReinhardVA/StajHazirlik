using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.DTOs;
using PetStore.Models;
using BCrypt.Net;

namespace PetStore.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterDTO userRequest)
        {

            var validationError = await ValidateUserAsync(userRequest);

            if (validationError != null)
            {
                return BadRequest($"Error for user {userRequest.Username}: {validationError}");
            }

            var user = MapToUser(userRequest);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User Created!!");
        }

        [HttpPost("createWithList")]
        [HttpPost("createWithArray")]
        public async Task<IActionResult> CreateUsers(List<RegisterDTO> usersRequest)
        {
            foreach (var userRequest in usersRequest)
            {
                var validationError = await ValidateUserAsync(userRequest);
                if (validationError != null)
                {
                    return BadRequest($"Error for user {userRequest.Username}: {validationError}");
                }

                var user = MapToUser(userRequest);
                _context.Users.Add(user);
            }

            await _context.SaveChangesAsync();

            return Ok($"{usersRequest.Count} users created successfully!");
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser([FromRoute] string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        //login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.UsernameOrEmail || u.Email == loginRequest.UsernameOrEmail);
            if(user == null)
            {
                return NotFound("User not found!");
            }
            bool verifiedPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
            if (!verifiedPassword)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok($"Login successfull, welcome {user.Username}");
        }
        // Logout
        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string username, [FromBody] RegisterDTO updateRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound("User not found");

            var validationError = await ValidateUserAsync(updateRequest, user.Id);
            if (validationError != null)
            {
                return BadRequest(validationError);
            }
            MapToUser(updateRequest, user);

            await _context.SaveChangesAsync();
            return Ok("User updated successfully");
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            
            if (user == null) return NotFound("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("User deleted successfully");
        }

        private async Task<string?> ValidateUserAsync(RegisterDTO userRequest, int? currentUserId = null)
        {
            if (await _context.Users.AnyAsync(u => u.Email == userRequest.Email && u.Id != currentUserId))
            {
                // Email check
                return "Email is already registered";
            }

            if (await _context.Users.AnyAsync(u => u.Username == userRequest.Username && u.Id != currentUserId))
            {
                // Username check
                return "Username is already registered";
            }

            if (!string.IsNullOrEmpty(userRequest.PhoneNumber) &&
                await _context.Users.AnyAsync(u => u.PhoneNumber == userRequest.PhoneNumber && u.Id != currentUserId))
            {
                // Phone number check
                return "Phone number is already registered";
            }

            return null;
        }

        // DTO'dan Entity'ye dönüştürme
        private User MapToUser(RegisterDTO dto, User? existingUser = null)
        {
            var user = existingUser ?? new User();

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Username = dto.Username;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            return user;
        }
    }
}
