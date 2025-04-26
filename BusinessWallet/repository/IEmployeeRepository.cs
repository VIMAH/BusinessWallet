using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessWallet.models;

namespace BusinessWallet.repository
{
    public interface IEmployeeRepository
    {
        Task<Employee> CreateAsync(Employee entity);
        Task<Employee?> GetByIdAsync(Guid id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task UpdateAsync(Employee entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
