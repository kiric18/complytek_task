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

        public async Task<decimal> GetTotalBudgetAsync(int departmentId)
        {
            return await _context.Projects
                .Where(p => p.DepartmentId == departmentId)
                .SumAsync(p => p.Budget);
        }

        public async Task<bool> HasEmployeesAsync(int departmentId)
        {
            return await _context.Employees.AnyAsync(e => e.DepartmentId == departmentId);
        }

        public async Task<bool> HasProjectsAsync(int departmentId)
        {
            return await _context.Projects.AnyAsync(p => p.DepartmentId == departmentId);
        }
    }
}
