using AutoMapper;
using BusinessWallet.DTOs;
using BusinessWallet.models;
using BusinessWallet.repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessWallet.services
{
    /// <inheritdoc />
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<EmployeeDto>(entity);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(entities);
        }

        public async Task<EmployeeDto> CreateAsync(EmployeeCreateDto dto)
        {
            var entity = _mapper.Map<Employee>(dto);
            await _repository.AddAsync(entity);
            return _mapper.Map<EmployeeDto>(entity);
        }

        public async Task<EmployeeDto?> UpdateAsync(Guid id, EmployeeUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null) return null;

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);

            return _mapper.Map<EmployeeDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id) => await _repository.DeleteAsync(id);
    }
}
