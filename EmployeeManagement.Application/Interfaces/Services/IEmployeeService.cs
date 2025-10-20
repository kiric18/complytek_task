using EmployeeManagement.Application.DTOs.Employee;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto> GetByIdAsync(int id);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task UpdateAsync(int id, UpdateEmployeeDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<EmployeeProjectDto>> GetEmployeeProjectsAsync(int id);
    }
}
