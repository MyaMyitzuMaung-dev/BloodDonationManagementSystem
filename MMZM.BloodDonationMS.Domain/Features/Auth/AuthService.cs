using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MMZM.BloodDonationMS.Database.AppDbContextModels;
using MMZM.BloodDonationMS.Domain.Features.Auth;

namespace MMZM.BloodDonationMS.Domain.Features;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // 🔐 Register
    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var exist = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (exist != null)
        {
            return new RegisterResponse
            {
                IsSuccess = false,
                Message = "Email already exists"
            };
        }

        var user = new User
        {
            RoleId = request.RoleId,
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Phone = request.Phone,
            Address = request.Address,
            BloodGroup = request.BloodGroup,
            IsAvailable = true,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new RegisterResponse
        {
            IsSuccess = true,
            Message = "Register successful"
        };
    }

    // 🔑 Login
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == request.Email && x.IsDeleted == false);

        if (user == null)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }

        var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isValid)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = "Invalid password"
            };
        }

        var token = GenerateJwtToken(user);

        return new LoginResponse
        {
            IsSuccess = true,
            Message = "Login successful",
            Token = token,
            UserId = user.UserId,
            Role = user.Role?.RoleName
        };
    }

    // 🔐 JWT Generator
    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}