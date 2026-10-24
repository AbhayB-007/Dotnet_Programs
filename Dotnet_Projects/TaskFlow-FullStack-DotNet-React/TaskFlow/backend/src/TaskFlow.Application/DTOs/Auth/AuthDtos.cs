namespace TaskFlow.Application.DTOs.Auth;

public record RegisterRequest(string FullName, string Email, string Password);

public record LoginRequest(string Email, string Password);

public record RefreshRequest(string RefreshToken);

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAtUtc,
    string UserId,
    string FullName,
    string Email,
    string Role);
