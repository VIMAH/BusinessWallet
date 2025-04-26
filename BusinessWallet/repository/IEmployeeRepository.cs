using BusinessWallet.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessWallet.repository
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(Guid id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task AddAsync(Employee entity);
        Task UpdateAsync(Employee entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
