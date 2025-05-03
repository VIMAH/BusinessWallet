using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessWallet.data;
using BusinessWallet.models;
using Microsoft.EntityFrameworkCore;

namespace BusinessWallet.repository
{
    public class EmployeeRoleChallengeRepository : IEmployeeRoleChallengeRepository
    {
        private readonly DataContext _context;

        public EmployeeRoleChallengeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(EmployeeRoleChallenge challenge)
        {
            await _context.EmployeeRoleChallenges.AddAsync(challenge);
        }

        public async Task<EmployeeRoleChallenge?> GetValidChallengeAsync(Guid employeeId, Guid roleId)
        {
            return await _context.EmployeeRoleChallenges
                .Where(c => c.EmployeeId == employeeId &&
                            c.RoleId == roleId &&
                            !c.IsUsed &&
                            c.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
