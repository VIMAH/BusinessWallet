using Microsoft.AspNetCore.Mvc;
using BusinessWallet.DTOs;
using BusinessWallet.services;

namespace BusinessWallet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto dto)
        {
            var role = await _roleService.CreateRoleAsync(dto);
            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var success = await _roleService.DeleteRoleAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] RoleUpdateDto dto)
        {
            var role = await _roleService.UpdateRoleAsync(id, dto);
            if (role == null) return NotFound();
            return Ok(role);
        }
    }
}
