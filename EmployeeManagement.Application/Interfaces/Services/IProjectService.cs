using EmployeeManagement.Application.DTOs.Project;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<ProjectDto> GetByIdAsync(Guid id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto);
        Task UpdateAsync(Guid id, UpdateProjectDto dto);
        Task DeleteAsync(Guid id);
        Task AssignEmployeeAsync(AssignEmployeeToProjectDto dto);
        Task RemoveEmployeeAsync(Guid projectId, Guid employeeId);
    }
}
