namespace Application.DTOs.Identity.RecoveryPasswordDtos
{
    public class ForgotPasswordResponse
    {
        public string TokenHash { get; set; } = null!;
        public string ResetLink { get; set; } = null!;
    }
}
