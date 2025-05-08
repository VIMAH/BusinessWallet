using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessWallet.models;
using BusinessWallet.models.Enums;

namespace BusinessWallet.repository
{
    /// <summary>
    /// Abstraheert alle database-operaties die de Auth-flow nodig heeft.
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Haalt alle medewerkers op, inclusief hun rollen.
        /// </summary>
        Task<List<Employee>> GetEmployeesWithRolesAsync();

        /// <summary>
        /// Controleert of er een <see cref="PolicyRule"/> bestaat
        /// die de gevraagde actie toestaat voor de opgegeven credential.
        /// </summary>
        Task<bool> HasAllowedPolicyAsync(
            ActionTypeEnum action,
            string credentialKey,
            Employee employee,
            Role role);

        /// <summary>
        /// Schrijft een <see cref="AuthorizationLog"/> weg.
        /// </summary>
        Task AddAuthorizationLogAsync(AuthorizationLog log);

        /// <summary>
        /// Persist alle pending wijzigingen.
        /// </summary>
        Task SaveChangesAsync();
    }
}
