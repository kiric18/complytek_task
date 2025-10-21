using EmployeeManagement.Application.DTOs.Project;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            return Ok(project);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
        {
            var project = await _projectService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
        {
            await _projectService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _projectService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("assign-employee")]
        public async Task<IActionResult> AssignEmployee(AssignEmployeeToProjectDto dto)
        {
            await _projectService.AssignEmployeeAsync(dto);
            return Ok(new { message = "Employee successfully assigned to project." });
        }

        [HttpDelete("{projectId}/employees/{employeeId}")]
        public async Task<IActionResult> RemoveEmployee(int projectId, int employeeId)
        {
            await _projectService.RemoveEmployeeAsync(projectId, employeeId);
            return Ok(new { message = "Employee successfully removed from project." });
        }
    }
}
