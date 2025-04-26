using System;
using BusinessWallet.models.Enums;

namespace BusinessWallet.DTOs
{
    /// <summary>Alleen niet-null velden worden geüpdatet.</summary>
    public class EmployeeUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Voorvoegsel { get; set; }
        public string? Voorletters { get; set; }
        public string? BirthName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool? OlderThan18 { get; set; }
        public string? BirthPlace { get; set; }
        public string? BirthCountry { get; set; }
        public bool? Married { get; set; }

        public string? LegalName { get; set; }
        public string? Category { get; set; }
        public string? City { get; set; }
        public string? Kvk { get; set; }
        public string? Position { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public VerificationState? VerificationState { get; set; }
        public EmployeeState? EmployeeState { get; set; }
        public string? PublicKey { get; set; }

        public DateTime? LaatsteAanmelding { get; set; }
    }
}
