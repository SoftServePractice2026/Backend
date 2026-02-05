namespace Application.Dtos;

public record IdentityDetailsDto(
    Guid Id, 
    string FirstName, 
    string LastName,
    string Email,
    DateTime BirthDate,
    ICollection<string> Roles);