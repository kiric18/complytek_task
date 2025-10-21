using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();
        }

        public override async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
        {
            var query = _context.Employees.Where(e => e.Email == email);

            if (excludeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<EmployeeProject>> GetEmployeeProjectsAsync(Guid employeeId)
        {
            return await _context.EmployeeProjects
                .Include(ep => ep.Project)
                .Where(ep => ep.EmployeeId == employeeId)
                .ToListAsync();
        }
    }

}
