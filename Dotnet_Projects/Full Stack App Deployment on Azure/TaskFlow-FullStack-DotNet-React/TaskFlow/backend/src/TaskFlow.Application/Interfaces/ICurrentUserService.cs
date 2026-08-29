using System;

namespace TaskFlow.Application.Interfaces;

// Abstracts away HttpContext so services never need to reference ASP.NET Core directly.
public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Role { get; }
    bool IsAdmin { get; }
}
