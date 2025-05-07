using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.DTOs
{
    public class AuthRequestChallengeDto
    {
        [Required]
        public string CallbackUrl { get; set; } = string.Empty;
    }
}
