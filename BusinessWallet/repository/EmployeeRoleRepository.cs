using System;
using System.Threading.Tasks;
using BusinessWallet.data;
using BusinessWallet.models;
using Microsoft.EntityFrameworkCore;

namespace BusinessWallet.repository
{
    public class EmployeeRoleRepository : IEmployeeRoleRepository
    {
        private readonly DataContext _context;

        public EmployeeRoleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<EmployeeRole?> GetByIdsAsync(Guid employeeId, Guid roleId)
            => await _context.EmployeeRoles
                             .SingleOrDefaultAsync(er =>
                                 er.EmployeeId == employeeId && er.RoleId == roleId);

        public async Task CreateAsync(EmployeeRole entity)
        {
            await _context.EmployeeRoles.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmployeeRole entity)
        {
            _context.EmployeeRoles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(EmployeeRole entity)
        {
            _context.EmployeeRoles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
