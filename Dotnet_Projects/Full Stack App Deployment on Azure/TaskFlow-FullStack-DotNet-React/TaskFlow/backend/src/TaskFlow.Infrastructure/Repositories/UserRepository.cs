using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.Users;
using TaskFlow.Application.Interfaces;
using TaskFlow.Infrastructure.Identity;

namespace TaskFlow.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UserRepository(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    public async Task<List<UserDto>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await _userManager.Users.OrderBy(u => u.FullName).ToListAsync(ct);
        var result = new List<UserDto>();
        foreach (var u in users)
        {
            var roles = await _userManager.GetRolesAsync(u);
            result.Add(new UserDto(u.Id, u.FullName, u.Email!, roles.FirstOrDefault() ?? "User", u.IsActive, u.CreatedAtUtc));
        }
        return result;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var u = await _userManager.FindByIdAsync(id.ToString());
        if (u is null) return null;
        var roles = await _userManager.GetRolesAsync(u);
        return new UserDto(u.Id, u.FullName, u.Email!, roles.FirstOrDefault() ?? "User", u.IsActive, u.CreatedAtUtc);
    }

    public async Task<Dictionary<Guid, string>> GetNamesByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var idList = ids.Distinct().ToList();
        return await _userManager.Users
            .Where(u => idList.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.FullName, ct);
    }
}
