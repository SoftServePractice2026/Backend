namespace Application.Dtos.Identity;

public record UpdateUserDto(
    string FirstName,
    string LastName,
    DateTime BirthDate,
    string? PhoneNumber);