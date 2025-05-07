using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.DTOs
{
    public class AuthRequestValidateDto
    {
        [Required]
        public Guid ChallengeId { get; set; }

        [Required]
        public string Signature { get; set; } = string.Empty;
    }
}
