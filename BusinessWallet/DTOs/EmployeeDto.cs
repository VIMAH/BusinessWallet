namespace BusinessWallet.DTOs
{
    /// <summary>
    /// Data Transfer Object voor het uitlezen van een werknemer.
    /// </summary>
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string? Voorletters { get; set; }
        public string? Voorvoegsel { get; set; }
        public string LastName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public DateTime? LaatsteAanmelding { get; set; }
        public string? BirthCountry { get; set; }
    }
}
