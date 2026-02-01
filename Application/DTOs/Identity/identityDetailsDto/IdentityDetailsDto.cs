namespace Application.Dtos;

public record IdentityDetailsDto(
    Guid Id, 
    string Name, 
    string Email);