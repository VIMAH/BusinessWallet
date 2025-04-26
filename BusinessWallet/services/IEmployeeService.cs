using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessWallet.DTOs;

namespace BusinessWallet.services
{
    public interface IEmployeeService
    {
        Task<EmployeeReadDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<EmployeeReadDto>> GetAllAsync();
        Task<EmployeeReadDto> CreateAsync(EmployeeCreateDto dto);
        Task<EmployeeReadDto?> UpdateAsync(Guid id, EmployeeUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
