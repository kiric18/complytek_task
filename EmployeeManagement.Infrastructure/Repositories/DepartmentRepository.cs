using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagement.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<decimal> GetTotalBudgetAsync(Guid departmentId)
        {
            return await _context.Departments
                .Where(d => d.Id == departmentId)
                .SelectMany(d => d.Employees)
                .SelectMany(e => e.EmployeeProjects)
                .Select(ep => ep.Project)
                .Distinct()
                .SumAsync(p => p.Budget);
        }

        public async Task<bool> HasEmployeesAsync(Guid departmentId)
        {
            return await _context.Employees.AnyAsync(e => e.DepartmentId == departmentId);
        }

        public async Task<bool> HasProjectsAsync(Guid departmentId)
        {
            return await _context.EmployeeProjects.AnyAsync(p => p.Employee.DepartmentId == departmentId);
        }
    }
}
