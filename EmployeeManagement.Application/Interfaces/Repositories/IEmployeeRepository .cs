using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task<IEnumerable<EmployeeProject>> GetEmployeeProjectsAsync(int employeeId);
    }
}
