using EmployeeManagement.Application.DTOs.Project;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<ProjectDto> GetByIdAsync(int id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto);
        Task UpdateAsync(int id, UpdateProjectDto dto);
        Task DeleteAsync(int id);
        Task AssignEmployeeAsync(AssignEmployeeToProjectDto dto);
        Task RemoveEmployeeAsync(int projectId, int employeeId);
    }
}
