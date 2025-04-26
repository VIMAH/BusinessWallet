using BusinessWallet.data;
using BusinessWallet.models;
using Microsoft.EntityFrameworkCore;

namespace BusinessWallet.repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _db;
        public EmployeeRepository(DataContext db) => _db = db;

        public async Task<Employee> AddAsync(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> GetByIdAsync(Guid id) =>
            await _db.Employees.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<Employee>> GetAllAsync() =>
            await _db.Employees.AsNoTracking().ToListAsync();
    }
}
