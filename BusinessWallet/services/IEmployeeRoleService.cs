using System.Threading.Tasks;
using BusinessWallet.DTOs;

namespace BusinessWallet.services
{
    /// <summary>
    /// Service-laag voor de API-endpoints (POST/PUT/DELETE) op EmployeeRole.
    /// </summary>
    public interface IEmployeeRoleService
    {
        Task AssignRoleAsync(EmployeeRoleCreateDto dto);   // POST
        Task UpdateRoleAsync(EmployeeRoleUpdateDto dto);   // PUT
        Task DeleteRoleAsync(EmployeeRoleDeleteDto dto);   // DELETE
    }
}
