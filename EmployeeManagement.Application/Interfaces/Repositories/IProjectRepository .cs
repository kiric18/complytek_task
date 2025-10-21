using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<bool> IsEmployeeAssignedAsync(Guid employeeId, Guid projectId);
        Task AssignEmployeeAsync(EmployeeProject employeeProject);
        Task RemoveEmployeeAsync(Guid employeeId, Guid projectId);
    }
}
