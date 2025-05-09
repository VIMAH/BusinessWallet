using BusinessWallet.DTOs;
using BusinessWallet.models;
using BusinessWallet.repository;
using Microsoft.EntityFrameworkCore;

namespace BusinessWallet.services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> CreateRoleAsync(RoleCreateDto dto)
        {
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                // 🛡️ IsSystemRole NIET via API instellen, default blijft false
            };

            await _roleRepository.AddAsync(role);
            return role;
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null) return false;

            await _roleRepository.DeleteAsync(role);
            return true;
        }

        public async Task<Role?> UpdateRoleAsync(Guid roleId, RoleUpdateDto dto)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.Name))
                role.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                role.Description = dto.Description;

            // 🛡️ Geen update van IsSystemRole via de API

            role.UpdatedAt = DateTime.UtcNow;
            await _roleRepository.UpdateAsync(role);

            return role;
        }

        public async Task<Role?> GetRoleByIdAsync(Guid id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllAsync();
        }
    }
}
