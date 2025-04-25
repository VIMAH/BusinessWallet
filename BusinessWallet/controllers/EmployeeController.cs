using BusinessWallet.DTOs;
using BusinessWallet.services;
using Microsoft.AspNetCore.Mvc;

namespace BusinessWallet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        public EmployeeController(IEmployeeService service) => _service = service;

        /// POST /api/employee
        [HttpPost]
        public async Task<ActionResult<EmployeeReadDTO>> Create([FromBody] EmployeeCreateDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// GET /api/employee/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeReadDTO>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

    }
}
