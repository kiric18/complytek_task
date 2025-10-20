using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Department)
                .ToListAsync();
        }

        public override async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IsEmployeeAssignedAsync(int employeeId, int projectId)
        {
            return await _context.EmployeeProjects
                .AnyAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);
        }

        public async Task AssignEmployeeAsync(EmployeeProject employeeProject)
        {
            await _context.EmployeeProjects.AddAsync(employeeProject);
        }

        public async Task RemoveEmployeeAsync(int employeeId, int projectId)
        {
            var employeeProject = await _context.EmployeeProjects
                .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);

            if (employeeProject != null)
            {
                _context.EmployeeProjects.Remove(employeeProject);
            }
        }
    }
}
