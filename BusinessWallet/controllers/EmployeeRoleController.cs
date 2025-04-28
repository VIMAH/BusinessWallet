using System;
using System.Threading.Tasks;
using BusinessWallet.DTOs;
using BusinessWallet.services;
using BusinessWallet.utils;      //  ➜  voor ResourceNotFoundException
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BusinessWallet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeRoleController : ControllerBase
    {
        private readonly IEmployeeRoleService _service;
        private readonly ILogger<EmployeeRoleController> _logger;

        public EmployeeRoleController(
            IEmployeeRoleService service,
            ILogger<EmployeeRoleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /* -------------------------------------------------------------
         * POST: api/EmployeeRole
         * Body: EmployeeRoleCreateDto
         * Toekennen van een rol aan een medewerker
         * ----------------------------------------------------------- */
        [HttpPost]
        public async Task<IActionResult> AssignRole([FromBody] EmployeeRoleCreateDto dto)
        {
            await _service.AssignRoleAsync(dto);
            return Created(string.Empty, dto);   // 201 + payload
        }

        /* -------------------------------------------------------------
         * PUT: api/EmployeeRole
         * Body: EmployeeRoleUpdateDto
         * Wijzigen van (of vervangen door) een rol
         * ----------------------------------------------------------- */
        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] EmployeeRoleUpdateDto dto)
        {
            try
            {
                await _service.UpdateRoleAsync(dto);
                return NoContent();              // 204
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);     // 404
            }
        }

        /* -------------------------------------------------------------
         * DELETE: api/EmployeeRole/{employeeId}/{roleId}
         * Verwijdert de koppel­rij voor de combinatie
         * ----------------------------------------------------------- */
        [HttpDelete("{employeeId:guid}/{roleId:guid}")]
        public async Task<IActionResult> DeleteRole(Guid employeeId, Guid roleId)
        {
            try
            {
                var dto = new EmployeeRoleDeleteDto
                {
                    EmployeeId = employeeId,
                    RoleId = roleId
                };

                await _service.DeleteRoleAsync(dto);
                return NoContent();              // 204
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);     // 404
            }
        }
    }
}
