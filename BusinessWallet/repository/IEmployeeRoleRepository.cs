using System;
using System.Threading.Tasks;
using BusinessWallet.models;

namespace BusinessWallet.repository
{
    /// <summary>
    /// Datatoegang voor de koppel-entiteit <see cref="EmployeeRole"/>.
    /// </summary>
    public interface IEmployeeRoleRepository
    {
        Task<EmployeeRole?> GetByIdsAsync(Guid employeeId, Guid roleId);
        Task CreateAsync(EmployeeRole entity);
        Task UpdateAsync(EmployeeRole entity);
        Task DeleteAsync(EmployeeRole entity);
    }
}
