namespace BusinessWallet.DTOs
{
    public class EmployeeCreateDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        // … voeg extra velden toe die extern invulbaar mogen zijn
    }
}
