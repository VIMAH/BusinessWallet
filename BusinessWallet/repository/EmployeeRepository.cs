using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessWallet.data;
using BusinessWallet.models;
using Microsoft.EntityFrameworkCore;

namespace BusinessWallet.repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _ctx;
        public EmployeeRepository(DataContext ctx) => _ctx = ctx;

        public async Task<Employee> CreateAsync(Employee entity)
        {
            _ctx.Employees.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public Task<Employee?> GetByIdAsync(Guid id) =>
            _ctx.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<Employee>> GetAllAsync() =>
            await _ctx.Employees.AsNoTracking().ToListAsync();

        public async Task UpdateAsync(Employee entity)
        {
            _ctx.Employees.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var e = await _ctx.Employees.FindAsync(id);
            if (e is null) return false;
            _ctx.Employees.Remove(e);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
