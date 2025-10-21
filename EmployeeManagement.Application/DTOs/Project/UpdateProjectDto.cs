using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.DTOs.Project
{
    public class UpdateProjectDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Budget { get; set; }
    }
}
