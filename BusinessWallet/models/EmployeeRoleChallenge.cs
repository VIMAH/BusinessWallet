using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWallet.models
{
    /// <summary>
    /// Tijdelijke challenge voor authenticatie via public/private key, gekoppeld aan een specifieke rol.
    /// </summary>
    public class EmployeeRoleChallenge
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [Required]
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;

        /// <summary>
        /// De unieke challenge string die naar de client wordt gestuurd.
        /// </summary>
        [Required]
        [MaxLength(512)]
        public string Challenge { get; set; } = null!;

        /// <summary>
        /// Tijdstip waarop de challenge is aangemaakt.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Tijdstip waarop de challenge verloopt (bijvoorbeeld 5 minuten na aanmaak).
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Optioneel: markeren of de challenge al gebruikt is (voor extra veiligheid).
        /// </summary>
        public bool IsUsed { get; set; } = false;
    }
}
