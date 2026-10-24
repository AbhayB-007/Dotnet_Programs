using System;
using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Infrastructure.Identity;

// Extends ASP.NET Core Identity's built-in user rather than reinventing
// password hashing / lockout / email confirmation from scratch.
public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
