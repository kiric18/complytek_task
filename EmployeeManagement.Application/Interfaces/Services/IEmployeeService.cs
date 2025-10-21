using EmployeeManagement.Application.DTOs.Employee;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto> GetByIdAsync(Guid id);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task UpdateAsync(Guid id, UpdateEmployeeDto dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<EmployeeProjectDto>> GetEmployeeProjectsAsync(Guid id);
    }
}
