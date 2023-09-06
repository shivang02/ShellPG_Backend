using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShellPG_Backend.Data;
using ShellPG_Backend.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShellPG_Backend.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                                       new SymmetricSecurityKey(key),
                                                          SecurityAlgorithms.HmacSha256Signature
                                                                         )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Implementing RegisterUser method

        public async Task<User> RegisterUser(User user)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Username == user.Username);
            if (userExists)
            {
                throw new Exception("User already exists");
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.PasswordHash = passwordHash;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Implementing LoginUser method

        public async Task<User> LoginUser(User user)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Username == user.Username);
            if (!userExists)
            {
                throw new Exception("User does not exist");
            }
            var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            var passwordValid = BCrypt.Net.BCrypt.Verify(user.PasswordHash, userFromDb.PasswordHash);
            if (!passwordValid)
            {
                throw new Exception("Invalid credentials");
            }
            return userFromDb;
        }
    }
}
