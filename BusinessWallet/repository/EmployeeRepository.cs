using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessWallet.models;
using BusinessWallet.data;            // <-- jouw DbContext-namespace
                                      //    (pas aan als hij anders heet)

namespace BusinessWallet.repository
{
    /// <summary>
    /// EF Core-implementatie van IEmployeeRepository.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _ctx;

        public EmployeeRepository(DataContext ctx) => _ctx = ctx;

        public async Task<Employee?> GetByIdAsync(Guid id) =>
            await _ctx.Employees.FindAsync(id);

        public async Task<IEnumerable<Employee>> GetAllAsync() =>
            await _ctx.Employees.ToListAsync();

        public async Task AddAsync(Employee entity)
        {
            await _ctx.Employees.AddAsync(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee entity)
        {
            _ctx.Employees.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _ctx.Employees.FindAsync(id);
            if (entity is null) return false;

            _ctx.Employees.Remove(entity);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
