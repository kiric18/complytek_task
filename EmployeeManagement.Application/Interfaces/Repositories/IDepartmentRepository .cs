using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<decimal> GetTotalBudgetAsync(Guid departmentId);
        Task<bool> HasEmployeesAsync(Guid departmentId);
        Task<bool> HasProjectsAsync(Guid departmentId);
    }
}
