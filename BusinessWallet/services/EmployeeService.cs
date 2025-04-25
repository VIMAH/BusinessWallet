using AutoMapper;
using BusinessWallet.DTOs;
using BusinessWallet.models;
using BusinessWallet.models.Enums;
using BusinessWallet.repository;

namespace BusinessWallet.services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<EmployeeReadDTO> CreateAsync(EmployeeCreateDTO dto)
        {
            var employee = _mapper.Map<Employee>(dto);

            // defaults
            employee.VerificationState = VerificationState.Unverified;
            employee.EmployeeState = EmployeeState.Inactive;

            var saved = await _repo.AddAsync(employee);
            return _mapper.Map<EmployeeReadDTO>(saved);
        }

        public async Task<EmployeeReadDTO?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<EmployeeReadDTO>(entity);
        }

        public async Task<IEnumerable<EmployeeReadDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeReadDTO>>(list);
        }
    }
}
