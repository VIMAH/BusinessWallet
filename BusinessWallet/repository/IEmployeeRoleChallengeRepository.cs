using System;
using System.Threading.Tasks;
using BusinessWallet.models;

namespace BusinessWallet.repository
{
    public interface IEmployeeRoleChallengeRepository
    {
        Task AddAsync(EmployeeRoleChallenge challenge);
        Task<EmployeeRoleChallenge?> GetValidChallengeAsync(Guid employeeId, Guid roleId);
        Task SaveChangesAsync();
    }
}
