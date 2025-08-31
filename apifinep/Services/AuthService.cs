using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using apifinep.Data;
using apifinep.DTOs;
using apifinep.Models;

namespace apifinep.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<UserDto?> RegisterAsync(RegisterDto registerDto);
        Task<UserDto?> GetUserByIdAsync(int userId);
        string GenerateJwtToken(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);

            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddHours(24); // Token válido por 24 horas

            return new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = expiresAt
            };
        }

        public async Task<UserDto?> RegisterAsync(RegisterDto registerDto)
        {
            // Verificar se username já existe
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return null; // Username já existe
            }

            // Verificar se email já existe
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return null; // Email já existe
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                PasswordHash = HashPassword(registerDto.Password),
                Role = registerDto.Role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurada");
            var issuer = jwtSettings["Issuer"] ?? "FinepAPI";
            var audience = jwtSettings["Audience"] ?? "FinepAPI";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string HashPassword(string password)
        {
            // Usar BCrypt para hash de senha (implementação simples para demonstração)
            // Em produção, considere usar BCrypt.Net-Next
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "FinepSalt"));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}