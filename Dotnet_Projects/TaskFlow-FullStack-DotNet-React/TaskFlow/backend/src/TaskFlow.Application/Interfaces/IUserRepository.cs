using TaskFlow.Application.DTOs.Users;

namespace TaskFlow.Application.Interfaces;

public interface IUserRepository
{
    Task<List<UserDto>> GetAllAsync(CancellationToken ct = default);
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Dictionary<Guid, string>> GetNamesByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
