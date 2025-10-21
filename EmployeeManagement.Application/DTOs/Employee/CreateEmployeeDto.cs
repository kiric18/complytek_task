using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public int DepartmentId { get; set; }
    }
}
