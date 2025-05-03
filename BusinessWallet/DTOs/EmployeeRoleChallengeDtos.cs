using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.DTOs
{
    /// <summary>
    /// DTO voor het aanvragen van een challenge.
    /// </summary>
    public class ChallengeRequestDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }

    /// <summary>
    /// DTO voor het teruggeven van een challenge.
    /// </summary>
    public class ChallengeResponseDto
    {
        public string Challenge { get; set; } = null!;
    }

    /// <summary>
    /// DTO voor het valideren van een gesigneerde challenge.
    /// </summary>
    public class ChallengeValidationRequestDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public string Signature { get; set; } = null!;
    }

    /// <summary>
    /// Optioneel: DTO voor het resultaat van de validatie.
    /// </summary>
    public class ChallengeValidationResponseDto
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; }
    }
}
