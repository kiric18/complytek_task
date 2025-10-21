using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.Exceptions;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                OfficeLocation = d.OfficeLocation
            });
        }

        public async Task<DepartmentDto> GetByIdAsync(Guid id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);

            if (department == null)
                throw new NotFoundException(nameof(Department), id);

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                OfficeLocation = department.OfficeLocation
            };
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            var department = new Department
            {
                Name = dto.Name,
                OfficeLocation = dto.OfficeLocation
            };

            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                OfficeLocation = department.OfficeLocation
            };
        }

        public async Task UpdateAsync(Guid id, UpdateDepartmentDto dto)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);

            if (department == null)
                throw new NotFoundException(nameof(Department), id);

            department.Name = dto.Name;
            department.OfficeLocation = dto.OfficeLocation;

            await _unitOfWork.Departments.UpdateAsync(department);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);

            if (department == null)
                throw new NotFoundException(nameof(Department), id);

            if (await _unitOfWork.Departments.HasEmployeesAsync(id))
                throw new BusinessRuleException("Cannot delete department with assigned employees.");

            if (await _unitOfWork.Departments.HasProjectsAsync(id))
                throw new BusinessRuleException("Cannot delete department with assigned projects.");

            await _unitOfWork.Departments.DeleteAsync(department);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalBudgetAsync(Guid id)
        {
            if (!await _unitOfWork.Departments.ExistsAsync(id))
                throw new NotFoundException(nameof(Department), id);

            return await _unitOfWork.Departments.GetTotalBudgetAsync(id);
        }
    }
}
