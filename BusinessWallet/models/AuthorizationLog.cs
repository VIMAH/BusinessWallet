using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWallet.models
{
    public class AuthorizationLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; } = null!;

        [Required]
        public Guid RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; } = null!;

        [Required, MaxLength(100)]
        public string RequestedBy { get; set; } = string.Empty; // Bijv. ING, interne module, etc.

        [Required, MaxLength(50)]
        public string Action { get; set; } = string.Empty;  // View, Issue, Verify

        [Required, MaxLength(150)]
        public string CredentialKey { get; set; } = string.Empty;  // Bijv. KvkNummer

        public string? AttributesJson { get; set; } // Optioneel: extra gevraagde gegevens

        public bool Result { get; set; } // true = toegestaan, false = geweigerd

        public string? Reason { get; set; } // Eventuele extra uitleg bij weigering

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
