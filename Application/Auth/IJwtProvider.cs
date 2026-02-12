namespace Application.Auth;

public interface IJwtProvider
{
    (string token, int expiry) GenerateToken(Guid userId, IEnumerable<string> roles);
    (string? token, int expiry) GenerateRefreshToken();
}