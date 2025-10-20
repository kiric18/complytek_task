using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public IDepartmentRepository Departments { get; }
        public IEmployeeRepository Employees { get; }
        public IProjectRepository Projects { get; }

        public UnitOfWork(
            AppDbContext context,
            IDepartmentRepository departments,
            IEmployeeRepository employees,
            IProjectRepository projects)
        {
            _context = context;
            Departments = departments;
            Employees = employees;
            Projects = projects;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
