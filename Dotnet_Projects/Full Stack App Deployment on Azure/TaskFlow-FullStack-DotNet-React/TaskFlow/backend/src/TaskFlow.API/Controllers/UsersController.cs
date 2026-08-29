using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs.Users;
using TaskFlow.Application.Interfaces;
using TaskFlow.Infrastructure.Identity;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)] // whole controller is admin-only: user management is an admin capability
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var users = await _userRepository.GetAllAsync(ct);
        return Ok(ApiResponse<List<UserDto>>.Ok(users));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(id, ct);
        if (user is null) return NotFound(ApiResponse<object>.Fail("User not found."));
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpPut("{id:guid}/role")]
    public async Task<IActionResult> UpdateRole(Guid id, UpdateUserRoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null) return NotFound(ApiResponse<object>.Fail("User not found."));

        if (request.Role != Roles.Admin && request.Role != Roles.User)
            return BadRequest(ApiResponse<object>.Fail("Role must be Admin or User."));

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, request.Role);

        return Ok(ApiResponse<object>.Ok(new { }, "Role updated."));
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> SetActive(Guid id, SetUserActiveRequest request)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null) return NotFound(ApiResponse<object>.Fail("User not found."));

        user.IsActive = request.IsActive;
        await _userManager.UpdateAsync(user);

        return Ok(ApiResponse<object>.Ok(new { }, request.IsActive ? "User activated." : "User deactivated."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null) return NotFound(ApiResponse<object>.Fail("User not found."));

        await _userManager.DeleteAsync(user);
        return Ok(ApiResponse<object>.Ok(new { }, "User deleted."));
    }
}
