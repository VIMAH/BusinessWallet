using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.models
{
    /// <summary>
    /// Beschrijft een rol (bijvoorbeeld HR, Finance).
    /// Alle rechten worden geregeld via de PolicyRule-tabel.
    /// </summary>
    public class Role
    {
        // ───── Primary Key ─────
        public Guid Id { get; set; } = Guid.NewGuid();

        // ───── Basisinfo ─────
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;  // Bijv. "HR", "Finance"

        [MaxLength(255)]
        public string? Description { get; set; }          // Optioneel: omschrijving van de rol

        public bool IsSystemRole { get; set; } = false;   // Bijv. true als dit een vaste systeemrol is

        // ───── Metadata ─────
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // ───── Navigatie ─────
        /// <summary>
        /// Navigatieproperty: koppelingen naar medewerkers die deze rol hebben.
        /// </summary>
        public ICollection<EmployeeRole>? EmployeeRoles { get; set; }

        // (Optioneel: je kunt hier een ICollection<PolicyRule> toevoegen
        // als je later PolicyRule uitbreidt met een directe Role-koppeling)
    }
}
