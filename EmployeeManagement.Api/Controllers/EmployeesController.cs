using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(Guid id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return Ok(employee);
        }

        [HttpPost("create")]
        public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
        {
            var employee = await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeDto dto)
        {
            await _employeeService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/projects")]
        public async Task<ActionResult<IEnumerable<EmployeeProjectDto>>> GetProjects(Guid id)
        {
            var projects = await _employeeService.GetEmployeeProjectsAsync(id);
            return Ok(projects);
        }
    }
}
