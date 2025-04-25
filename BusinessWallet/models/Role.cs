using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.models
{
    public class Role
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        // 🔐 Statistische rechten
        public bool CanStore { get; set; } = false; // credential_store
        public bool CanView { get; set; } = false; // credential_view
        public bool CanReceive { get; set; } = false; // credential_receive
        public bool CanPresent { get; set; } = false; // credential_present

        public bool IsSystemRole { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigatie
        public ICollection<EmployeeRole>? EmployeeRoles { get; set; }
    }
}
