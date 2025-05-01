using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.DTOs
{
    public class RoleCreateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        public bool CanIssue { get; set; } = false;
        public bool CanReceive { get; set; } = false;
        public bool CanStore { get; set; } = false;
        public bool CanView { get; set; } = false;
        public bool CanPresent { get; set; } = false;
        public bool CanVerify { get; set; } = false;
        public bool CanRevoke { get; set; } = false;
    }
}
