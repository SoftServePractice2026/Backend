namespace Application.Dtos.Identity;

public record AuthResponse(
    string Token,
    DateTime ExpiryDate,
    IdentityDetailsDto UserDetails,
    string? RefreshToken = null!,
    DateTime? ExpiryDateRefresh = null!
    );