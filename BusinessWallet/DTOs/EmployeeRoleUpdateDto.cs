namespace BusinessWallet.DTOs
{
    public class EmployeeRoleUpdateDto
    {
        public Guid EmployeeId { get; set; }
        public Guid CurrentRoleId { get; set; }
        public Guid NewRoleId { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
