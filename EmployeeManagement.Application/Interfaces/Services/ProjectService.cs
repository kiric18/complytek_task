using EmployeeManagement.Application.DTOs.Project;
using EmployeeManagement.Application.Exceptions;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRandomStringGenerator _randomStringGenerator;

        public ProjectService(
            IUnitOfWork unitOfWork,
            IRandomStringGenerator randomStringGenerator)
        {
            _unitOfWork = unitOfWork;
            _randomStringGenerator = randomStringGenerator;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();
            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Budget = p.Budget,
                ProjectCode = p.ProjectCode
            });
        }

        public async Task<ProjectDto> GetByIdAsync(Guid id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);

            if (project == null)
                throw new NotFoundException(nameof(Project), id);

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Budget = project.Budget,
                ProjectCode = project.ProjectCode
            };
        }

        // Implements Bonus Task 1: Transaction-based project creation
        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            try
            {
                // Begin transaction
                await _unitOfWork.BeginTransactionAsync();

                // Step 1: Create project with temporary code
                var project = new Project
                {
                    Name = dto.Name,
                    Budget = dto.Budget,
                    ProjectCode = "TEMP" // Temporary code
                };

                await _unitOfWork.Projects.AddAsync(project);
                await _unitOfWork.SaveChangesAsync();

                // Step 2: Generate random string from external service
                var randomString = await _randomStringGenerator.GenerateRandomStringAsync(8);

                // Step 3: Append ProjectId to the generated code
                project.ProjectCode = $"{randomString}-{project.Id}";

                await _unitOfWork.Projects.UpdateAsync(project);
                await _unitOfWork.SaveChangesAsync();

                // Commit transaction
                await _unitOfWork.CommitTransactionAsync();

                // Reload project with department
                project = await _unitOfWork.Projects.GetByIdAsync(project.Id);

                return new ProjectDto
                {
                    Id = project!.Id,
                    Name = project.Name,
                    Budget = project.Budget,
                    ProjectCode = project.ProjectCode
                };
            }
            catch (Exception)
            {
                // Rollback transaction on any error
                await _unitOfWork.RollbackTransactionAsync();
                throw new BusinessRuleException("An error occurred while creating the project. Transaction has been rolled back.");
            }
        }

        public async Task UpdateAsync(Guid id, UpdateProjectDto dto)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);

            if (project == null)
                throw new NotFoundException(nameof(Project), id);

            project.Name = dto.Name;
            project.Budget = dto.Budget;
            // Note: ProjectCode is not updated as it's unique and system-generated

            await _unitOfWork.Projects.UpdateAsync(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);

            if (project == null)
                throw new NotFoundException(nameof(Project), id);

            await _unitOfWork.Projects.DeleteAsync(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssignEmployeeAsync(AssignEmployeeToProjectDto dto)
        {
            if (!await _unitOfWork.Employees.ExistsAsync(dto.EmployeeId))
                throw new NotFoundException(nameof(Employee), dto.EmployeeId);

            if (!await _unitOfWork.Projects.ExistsAsync(dto.ProjectId))
                throw new NotFoundException(nameof(Project), dto.ProjectId);

            if (await _unitOfWork.Projects.IsEmployeeAssignedAsync(dto.EmployeeId, dto.ProjectId))
                throw new BusinessRuleException("Employee is already assigned to this project.");

            var employeeProject = new EmployeeProject
            {
                EmployeeId = dto.EmployeeId,
                ProjectId = dto.ProjectId,
                Role = dto.Role
            };

            await _unitOfWork.Projects.AssignEmployeeAsync(employeeProject);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveEmployeeAsync(Guid projectId, Guid employeeId)
        {
            if (!await _unitOfWork.Projects.IsEmployeeAssignedAsync(employeeId, projectId))
                throw new NotFoundException("Employee assignment to project not found.");

            await _unitOfWork.Projects.RemoveEmployeeAsync(employeeId, projectId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
