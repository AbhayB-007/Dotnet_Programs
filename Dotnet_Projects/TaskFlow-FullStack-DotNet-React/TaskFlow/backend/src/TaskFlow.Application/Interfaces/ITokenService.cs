using System;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface ITokenService
{
    (string token, DateTime expiresAtUtc) GenerateAccessToken(Guid userId, string email, string fullName, string role);
    string GenerateRefreshToken();
}
