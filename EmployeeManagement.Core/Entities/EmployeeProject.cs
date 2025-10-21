using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Core.Entities
{
    public class EmployeeProject
    {
        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Role { get; set; } = null!;

        public Project Project { get; set; } = null!;

        public Employee Employee { get; set; } = null!;
    }
}
