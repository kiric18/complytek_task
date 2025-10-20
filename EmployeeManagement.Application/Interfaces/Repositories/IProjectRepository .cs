using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<bool> IsEmployeeAssignedAsync(int employeeId, int projectId);
        Task AssignEmployeeAsync(EmployeeProject employeeProject);
        Task RemoveEmployeeAsync(int employeeId, int projectId);
    }
}
