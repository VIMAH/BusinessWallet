namespace BusinessWallet.models
{
    /// <summary>
    /// Many-to-many koppeltabel tussen Employee en Role.
    /// </summary>
    public class EmployeeRole
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
    }
}
