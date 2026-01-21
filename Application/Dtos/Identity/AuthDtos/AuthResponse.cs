namespace Application.Dtos.Identity;

public record AuthResponse(
    string Token,
    DateTime ExpiryDate,
    IdentityDetailsDto UserDetails
    );