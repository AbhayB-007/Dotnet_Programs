using System.Security.Claims;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Extensions;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;
    public CurrentUserService(IHttpContextAccessor accessor) => _accessor = accessor;

    public Guid? UserId
    {
        get
        {
            var raw = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(raw, out var id) ? id : null;
        }
    }

    public string? Role => _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

    public bool IsAdmin => Role == "Admin";
}
