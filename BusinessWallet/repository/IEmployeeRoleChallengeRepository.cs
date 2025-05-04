using System;
using System.Threading.Tasks;
using BusinessWallet.models;

namespace BusinessWallet.repository
{
    public interface IEmployeeRoleChallengeRepository
    {
        Task<EmployeeRoleChallenge?> GetValidChallengeAsync(Guid employeeId, Guid roleId);
        Task AddAsync(EmployeeRoleChallenge challenge);
        Task SaveChangesAsync();
    }
}
