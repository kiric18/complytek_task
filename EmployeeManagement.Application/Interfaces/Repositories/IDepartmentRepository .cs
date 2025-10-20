using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<decimal> GetTotalBudgetAsync(int departmentId);
        Task<bool> HasEmployeesAsync(int departmentId);
        Task<bool> HasProjectsAsync(int departmentId);
    }
}
