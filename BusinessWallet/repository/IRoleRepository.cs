using BusinessWallet.models;

namespace BusinessWallet.repository
{
    public interface IRoleRepository
    {
        Task AddAsync(Role role);
        Task<Role?> GetByIdAsync(Guid id);
        Task DeleteAsync(Role role);
        Task UpdateAsync(Role role);
        Task<IEnumerable<Role>> GetAllAsync();
    }
}
