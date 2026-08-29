namespace TaskFlow.Application.DTOs.Users;

public record UserDto(Guid Id, string FullName, string Email, string Role, bool IsActive, DateTime CreatedAtUtc);

public record UpdateUserRoleRequest(string Role);

public record SetUserActiveRequest(bool IsActive);

public record UpdateProfileRequest(string FullName);
