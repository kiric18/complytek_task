namespace EmployeeManagement.Application.DTOs.Employee
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}
