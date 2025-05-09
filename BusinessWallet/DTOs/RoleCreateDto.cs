using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.DTOs
{
    public class RoleCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
