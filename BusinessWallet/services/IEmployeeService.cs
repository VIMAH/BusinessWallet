using BusinessWallet.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessWallet.services
{
    /// <summary>
    /// Publieke business-interface voor Employees.
    /// </summary>
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto> CreateAsync(EmployeeCreateDto dto);
        Task<EmployeeDto?> UpdateAsync(Guid id, EmployeeUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
