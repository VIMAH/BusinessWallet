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
                CanIssue = dto.CanIssue,
                CanReceive = dto.CanReceive,
                CanStore = dto.CanStore,
                CanView = dto.CanView,
                CanPresent = dto.CanPresent,
                CanVerify = dto.CanVerify,
                CanRevoke = dto.CanRevoke,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
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

            if (dto.Name != null) role.Name = dto.Name;
            if (dto.Description != null) role.Description = dto.Description;
            if (dto.CanIssue.HasValue) role.CanIssue = dto.CanIssue.Value;
            if (dto.CanReceive.HasValue) role.CanReceive = dto.CanReceive.Value;
            if (dto.CanStore.HasValue) role.CanStore = dto.CanStore.Value;
            if (dto.CanView.HasValue) role.CanView = dto.CanView.Value;
            if (dto.CanPresent.HasValue) role.CanPresent = dto.CanPresent.Value;
            if (dto.CanVerify.HasValue) role.CanVerify = dto.CanVerify.Value;
            if (dto.CanRevoke.HasValue) role.CanRevoke = dto.CanRevoke.Value;

            role.UpdatedAt = DateTime.UtcNow;
            await _roleRepository.UpdateAsync(role);

            return role;
        }

        public async Task<Role?> GetRoleByIdAsync(Guid id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }
    }
}
