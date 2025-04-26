namespace BusinessWallet.DTOs
{
    /// <summary>
    /// DTO voor het bijwerken van een werknemer.
    /// </summary>
    public class EmployeeUpdateDto
    {
        public string FirstName { get; set; } = default!;
        public string? Voorletters { get; set; }
        public string? Voorvoegsel { get; set; }
        public string LastName { get; set; } = default!;
        public DateTime? LaatsteAanmelding { get; set; }
        public string? BirthCountry { get; set; }
    }
}
