using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.Interfaces;
using TaskFlow.Infrastructure.Data;
using TaskFlow.Infrastructure.Identity;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, AppDbContext context, IConfiguration config)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            return BadRequest(ApiResponse<object>.Fail("An account with this email already exists."));

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            EmailConfirmed = true // simplified for demo; wire up real email confirmation in production
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(ApiResponse<object>.Fail(string.Join(" ", result.Errors.Select(e => e.Description))));

        await _userManager.AddToRoleAsync(user, Roles.User); // everyone self-registers as a normal user

        var authResponse = await IssueTokensAsync(user);
        return Ok(ApiResponse<AuthResponse>.Ok(authResponse, "Registration successful."));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.IsActive)
            return Unauthorized(ApiResponse<object>.Fail("Invalid credentials or inactive account."));

        var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!validPassword)
            return Unauthorized(ApiResponse<object>.Fail("Invalid credentials."));

        var authResponse = await IssueTokensAsync(user);
        return Ok(ApiResponse<AuthResponse>.Ok(authResponse, "Login successful."));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var stored = _context.RefreshTokens.FirstOrDefault(t => t.Token == request.RefreshToken);
        if (stored is null || !stored.IsActive)
            return Unauthorized(ApiResponse<object>.Fail("Invalid or expired refresh token."));

        var user = await _userManager.FindByIdAsync(stored.UserId.ToString());
        if (user is null || !user.IsActive)
            return Unauthorized(ApiResponse<object>.Fail("Account no longer active."));

        // Rotate: revoke the old token, issue a brand new pair (mitigates replay attacks).
        stored.Revoked = true;
        var authResponse = await IssueTokensAsync(user);
        stored.ReplacedByToken = authResponse.RefreshToken;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<AuthResponse>.Ok(authResponse, "Token refreshed."));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshRequest request)
    {
        var stored = _context.RefreshTokens.FirstOrDefault(t => t.Token == request.RefreshToken);
        if (stored is not null)
        {
            stored.Revoked = true;
            await _context.SaveChangesAsync();
        }
        return Ok(ApiResponse<object>.Ok(new { }, "Logged out."));
    }

    private async Task<AuthResponse> IssueTokensAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? Roles.User;

        var (accessToken, expiresAt) = _tokenService.GenerateAccessToken(user.Id, user.Email!, user.FullName, role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        _context.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:RefreshTokenExpiryDays"]!))
        });
        await _context.SaveChangesAsync();

        return new AuthResponse(accessToken, refreshToken, expiresAt, user.Id.ToString(), user.FullName, user.Email!, role);
    }
}
