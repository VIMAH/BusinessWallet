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

        // 🔐 Credential acties
        public bool CanIssue { get; set; } = false;      // Uitgeven van credentials
        public bool CanReceive { get; set; } = false;    // Ontvangen van credentials
        public bool CanStore { get; set; } = false;      // Opslaan / beheren
        public bool CanView { get; set; } = false;       // Inzien van credentials
        public bool CanPresent { get; set; } = false;    // Presenteren van credentials
        public bool CanVerify { get; set; } = false;     // Verifiëren van gepresenteerde credentials
        public bool CanRevoke { get; set; } = false;     // Intrekken van credentials

        public bool IsSystemRole { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigatie
        public ICollection<EmployeeRole>? EmployeeRoles { get; set; }
    }
}
