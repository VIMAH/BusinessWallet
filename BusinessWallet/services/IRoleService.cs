using BusinessWallet.DTOs;
using BusinessWallet.models;

namespace BusinessWallet.services
{
    public interface IRoleService
    {
        Task<Role> CreateRoleAsync(RoleCreateDto dto);
        Task<bool> DeleteRoleAsync(Guid roleId);
        Task<Role?> UpdateRoleAsync(Guid roleId, RoleUpdateDto dto);
        Task<Role?> GetRoleByIdAsync(Guid id);
    }
}
