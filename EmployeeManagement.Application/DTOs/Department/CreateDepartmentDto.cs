using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.DTOs.Department
{
    public class CreateDepartmentDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string OfficeLocation { get; set; } = string.Empty;
    }
}
