using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.DTOs
{
    public class ChallengeValidationRequestDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public string Signature { get; set; } = null!;

        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Actie mag alleen letters bevatten.")]
        public string Action { get; set; } = null!;
    }
}
