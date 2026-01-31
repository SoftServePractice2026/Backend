using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Authentication;

public class JwtOptions
{
    
    [Required(AllowEmptyStrings = false)]
    [MinLength(32)]
    public string SecretKey { get; init; } = string.Empty;
    
    
    [Required]
    public string Issuer { get; init; } = string.Empty;
    
    [Required]
    public string Audience { get; init; } = string.Empty;
    
    
    [Range(1, 168)]
    public int ExpiresHours { get; init; }
}