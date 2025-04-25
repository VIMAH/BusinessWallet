using BusinessWallet.models.Enums;

namespace BusinessWallet.DTOs
{
    public class EmployeeReadDTO
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public VerificationState VerificationState { get; set; }
        public EmployeeState EmployeeState { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
