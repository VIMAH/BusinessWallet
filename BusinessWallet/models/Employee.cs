using System.ComponentModel.DataAnnotations;
using BusinessWallet.models.Enums;

namespace BusinessWallet.models
{
    /// <summary>
    /// Basisgegevens van een medewerker.
    /// (IsAdmin + navigatie staan in Employee.Admin.cs)
    /// </summary>
    public partial class Employee
    {
        // ───── Primary key ─────
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // ───── Persoonlijke info ─────
        [MaxLength(150)] public string? FirstName { get; set; }
        [MaxLength(150)] public string? LastName { get; set; }
        [MaxLength(150)] public string? BirthName { get; set; }
        [MaxLength(50)] public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool OlderThan18 { get; set; }
        [MaxLength(150)] public string? BirthPlace { get; set; }
        public bool Married { get; set; }

        // ───── Bedrijfsinfo ─────
        [MaxLength(200)] public string? LegalName { get; set; }
        [MaxLength(100)] public string? Category { get; set; }
        [MaxLength(100)] public string? City { get; set; }
        [MaxLength(50)] public string? Kvk { get; set; }
        [MaxLength(100)] public string? Position { get; set; }

        // ───── Contact ─────
        [EmailAddress] public string? Email { get; set; }
        [Phone] public string? PhoneNumber { get; set; }

        // ───── Wallet/status ─────
        public VerificationState VerificationState { get; set; } = VerificationState.Unverified;
        public EmployeeState EmployeeState { get; set; } = EmployeeState.Inactive;

        public string? PublicKey { get; set; }

        // ───── Timestamps ─────
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
