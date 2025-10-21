using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(departments);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<DepartmentDto>> GetById(Guid id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            return Ok(department);
        }

        [HttpPost("create")]
        public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto dto)
        {
            var department = await _departmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateDepartmentDto dto)
        {
            await _departmentService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _departmentService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/total-budget")]
        public async Task<ActionResult<object>> GetTotalBudget(Guid id)
        {
            var totalBudget = await _departmentService.GetTotalBudgetAsync(id);
            return Ok(new { departmentId = id, totalBudget });
        }
    }
}
