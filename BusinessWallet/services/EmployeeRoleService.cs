using System;
using System.Threading.Tasks;
using BusinessWallet.DTOs;
using BusinessWallet.models;
using BusinessWallet.repository;
using BusinessWallet.utils;   //  ➜  zorg dat ResourceNotFoundException bestaat

namespace BusinessWallet.services
{
    public class EmployeeRoleService : IEmployeeRoleService
    {
        private readonly IEmployeeRoleRepository _repository;

        public EmployeeRoleService(IEmployeeRoleRepository repository)
        {
            _repository = repository;
        }

        /* ---------- POST /employeeRoles ---------- */
        public async Task AssignRoleAsync(EmployeeRoleCreateDto dto)
        {
            var entity = new EmployeeRole
            {
                EmployeeId = dto.EmployeeId,
                RoleId = dto.RoleId,
                AssignedAt = dto.AssignedAt ?? DateTime.UtcNow,
                ExpiresAt = dto.ExpiresAt
            };

            await _repository.CreateAsync(entity);
        }

        /* ---------- PUT /employeeRoles ---------- */
        public async Task UpdateRoleAsync(EmployeeRoleUpdateDto dto)
        {
            var current = await _repository.GetByIdsAsync(dto.EmployeeId, dto.RoleId);

            if (current is null)
                throw new ResourceNotFoundException(
                    $"EmployeeRole met EmployeeId {dto.EmployeeId} en RoleId {dto.RoleId} niet gevonden.");

            // Business-regel: als RoleId wijzigt → oude record weg, nieuwe record terugplaatsen.
            if (dto.RoleId != current.RoleId)
            {
                await _repository.DeleteAsync(current);

                var replacement = new EmployeeRole
                {
                    EmployeeId = dto.EmployeeId,
                    RoleId = dto.RoleId,
                    AssignedAt = dto.AssignedAt ?? DateTime.UtcNow,
                    ExpiresAt = dto.ExpiresAt
                };
                await _repository.CreateAsync(replacement);
            }
            else
            {
                current.AssignedAt = dto.AssignedAt ?? current.AssignedAt;
                current.ExpiresAt = dto.ExpiresAt;
                await _repository.UpdateAsync(current);
            }
        }

        /* ---------- DELETE /employeeRoles ---------- */
        public async Task DeleteRoleAsync(EmployeeRoleDeleteDto dto)
        {
            var entity = await _repository.GetByIdsAsync(dto.EmployeeId, dto.RoleId);

            if (entity is null)
                throw new ResourceNotFoundException(
                    $"EmployeeRole met EmployeeId {dto.EmployeeId} en RoleId {dto.RoleId} niet gevonden.");

            await _repository.DeleteAsync(entity);
        }
    }
}
