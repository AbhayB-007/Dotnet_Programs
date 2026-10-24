using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public bool Revoked { get; set; } = false;
    public string? ReplacedByToken { get; set; }

    public bool IsActive => !Revoked && DateTime.UtcNow < ExpiresAtUtc;
}
