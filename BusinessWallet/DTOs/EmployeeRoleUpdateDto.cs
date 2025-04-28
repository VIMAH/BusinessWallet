namespace BusinessWallet.DTOs
{
    public class EmployeeRoleUpdateDto
    {
        public Guid EmployeeId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
