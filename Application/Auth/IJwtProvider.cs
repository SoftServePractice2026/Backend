namespace Application.Auth;

public interface IJwtProvider
{
    string GenerateToken(Guid userId, IEnumerable<string> roles);
    string? GenerateRefreshToken();
}