using Microsoft.AspNetCore.Mvc;
using BusinessWallet.DTOs;
using BusinessWallet.services;
using Swashbuckle.AspNetCore.Filters;
using BusinessWallet.utils;

namespace BusinessWallet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolicyRulesController : ControllerBase
    {
        private readonly IPolicyRulesService _service;

        public PolicyRulesController(IPolicyRulesService service)
        {
            _service = service;
        }

        [HttpPost]
        [SwaggerRequestExample(typeof(CreatePolicyRuleDto), typeof(CreatePolicyRuleDtoExample))]
        public async Task<IActionResult> CreatePolicyRule([FromBody] CreatePolicyRuleDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetPolicyRule), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPolicyRule(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolicyRule(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePolicyRule(Guid id, [FromBody] UpdatePolicyRuleDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
