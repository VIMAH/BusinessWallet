// File: DTOs/AuthRequestTokenDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.DTOs
{
    public class AuthRequestTokenDto
    {
        [Required]
        public Guid ChallengeId { get; set; }

        [Required]
        public string Signature { get; set; } = string.Empty;
    }
}
