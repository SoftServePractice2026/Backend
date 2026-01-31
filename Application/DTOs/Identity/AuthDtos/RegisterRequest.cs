namespace Application.Dtos.Identity;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateTime BirthDate);