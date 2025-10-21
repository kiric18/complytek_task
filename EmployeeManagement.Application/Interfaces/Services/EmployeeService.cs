using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Exceptions;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _unitOfWork.Employees.GetAllAsync();
            return employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Salary = e.Salary,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department.Name
            });
        }

        public async Task<EmployeeDto> GetByIdAsync(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);

            if (employee == null)
                throw new NotFoundException(nameof(Employee), id);

            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department.Name
            };
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            if (!await _unitOfWork.Departments.ExistsAsync(dto.DepartmentId))
                throw new NotFoundException(nameof(Department), dto.DepartmentId);

            if (await _unitOfWork.Employees.EmailExistsAsync(dto.Email))
                throw new ValidationException("An employee with this email already exists.");

            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentId
            };

            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            employee = await _unitOfWork.Employees.GetByIdAsync(employee.Id);

            return new EmployeeDto
            {
                Id = employee!.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department.Name
            };
        }

        public async Task UpdateAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);

            if (employee == null)
                throw new NotFoundException(nameof(Employee), id);

            if (!await _unitOfWork.Departments.ExistsAsync(dto.DepartmentId))
                throw new NotFoundException(nameof(Department), dto.DepartmentId);

            if (await _unitOfWork.Employees.EmailExistsAsync(dto.Email, id))
                throw new ValidationException("An employee with this email already exists.");

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.Salary = dto.Salary;
            employee.DepartmentId = dto.DepartmentId;

            await _unitOfWork.Employees.UpdateAsync(employee);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);

            if (employee == null)
                throw new NotFoundException(nameof(Employee), id);

            await _unitOfWork.Employees.DeleteAsync(employee);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeProjectDto>> GetEmployeeProjectsAsync(int id)
        {
            if (!await _unitOfWork.Employees.ExistsAsync(id))
                throw new NotFoundException(nameof(Employee), id);

            var employeeProjects = await _unitOfWork.Employees.GetEmployeeProjectsAsync(id);

            return employeeProjects.Select(ep => new EmployeeProjectDto
            {
                ProjectId = ep.ProjectId,
                ProjectName = ep.Project.Name,
                ProjectCode = ep.Project.ProjectCode,
                Role = ep.Role
            });
        }
    }
}
