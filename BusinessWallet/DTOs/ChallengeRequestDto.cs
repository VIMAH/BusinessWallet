using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.DTOs
{
    public class ChallengeRequestDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}
