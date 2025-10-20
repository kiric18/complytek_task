using EmployeeManagement.Application.DTOs.Department;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto> GetByIdAsync(int id);
        Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
        Task UpdateAsync(int id, UpdateDepartmentDto dto);
        Task DeleteAsync(int id);
        Task<decimal> GetTotalBudgetAsync(int id);
    }
}
