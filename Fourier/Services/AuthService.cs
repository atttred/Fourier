namespace Fourier.Services;
using Fourier.Repositories;
using Fourier.DTOs;
using Fourier.Models;
using Fourier.Models.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto model)
    {
        var existingUser = await _userRepository.GetByUserNameAsync(model.Username);
        if (existingUser != null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "User with this username already exists",
                Errors = new List<string> { "User with this username already exists" },
            };
        }

        if (model.Password != model.ConfirmPassword)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Passwords do not match",
                Errors = new List<string> { "Passwords do not match" },
            };
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = model.Username,
            HashedPassword = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        return new AuthResponseDto
        {
            Success = true,
            Message = "User created successfully",
            Username = user.UserName,
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto model)
    {
        var user = await _userRepository.GetByUserNameAsync(model.Username);
        if (user == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid username or password",
                Errors = new List<string> { "Invalid username" },
            };
        }

        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.HashedPassword))
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid username or password",
                Errors = new List<string> { "Invalid password" },
            };
        }

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Success = true,
            Token = token,
            Username = user.UserName,
        };
    }
}