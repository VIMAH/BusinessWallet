using Microsoft.AspNetCore.Mvc;
using BusinessWallet.DTOs;
using BusinessWallet.services;
using System;
using System.Threading.Tasks;

namespace BusinessWallet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        public EmployeeController(IEmployeeService service) => _service = service;

        // ---------- GET (single) ----------
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        // ---------- GET (list) -----------
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // ---------- POST -----------------
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetEmployee), new { id = result.Id }, result);
        }

        // ---------- PUT ------------------
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result is null ? NotFound() : Ok(result);
        }

        // ---------- DELETE ---------------
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
