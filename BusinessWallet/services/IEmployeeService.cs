using BusinessWallet.DTOs;

namespace BusinessWallet.services
{
    public interface IEmployeeService
    {
        Task<EmployeeReadDTO> CreateAsync(EmployeeCreateDTO dto);
        Task<IEnumerable<EmployeeReadDTO>> GetAllAsync();

        // ➕ Nieuw
        Task<EmployeeReadDTO?> GetByIdAsync(Guid id);
    }
}
