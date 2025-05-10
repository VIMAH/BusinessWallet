using System;
using System.ComponentModel.DataAnnotations;
using BusinessWallet.models.Enums;

namespace BusinessWallet.models
{
    /// <summary>
    /// Regelt welke actie is toegestaan voor welk doelwit, onder welke voorwaarden.
    /// </summary>
    public class PolicyRule
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public ActionTypeEnum Action { get; set; }  // Enum: View, Issue, Verify, etc.

        [Required, MaxLength(50)]
        public string TargetType { get; set; } = string.Empty;  // Bijvoorbeeld: Credential, CredentialType

        [Required, MaxLength(150)]
        public string TargetValue { get; set; } = string.Empty; // Bijvoorbeeld: KvkNummer, CompanyInfo

        [Required]
        public string ConditionJson { get; set; } = "{}"; // Bijvoorbeeld: { "Role": "HR" }

        [Required]
        public bool IsAllowed { get; set; } = true;

        public string? Message { get; set; } // Optioneel: uitleg bij toelating/weigering

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
