using BusinessWallet.models;

namespace BusinessWallet.repository
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddAsync(Employee employee);
        Task<IEnumerable<Employee>> GetAllAsync();

        // ➕ Nieuw
        Task<Employee?> GetByIdAsync(Guid id);
    }
}
