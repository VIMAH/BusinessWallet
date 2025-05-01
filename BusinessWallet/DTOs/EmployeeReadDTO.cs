using System;
using BusinessWallet.models.Enums;

namespace BusinessWallet.DTOs
{
    public class EmployeeReadDto
    {
        public Guid Id { get; set; }

        // Persoonlijke info
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Voorvoegsel { get; set; }
        public string? Voorletters { get; set; }
        public string FullName { get; set; } = default!;
        public string? BirthName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool OlderThan18 { get; set; }
        public string? BirthPlace { get; set; }
        public string? BirthCountry { get; set; }
        public bool Married { get; set; }

        // Bedrijfsinfo
        public string? LegalName { get; set; }
        public string? Category { get; set; }
        public string? City { get; set; }
        public string? Kvk { get; set; }
        public string? Position { get; set; }

        // Contact
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        // Wallet/status
        public VerificationState VerificationState { get; set; }
        public EmployeeState EmployeeState { get; set; }
        public string? PublicKey { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LaatsteAanmelding { get; set; }
    }
}
