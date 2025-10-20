using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Core.Entities
{
    public class EmployeeProject
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;


        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;


        [Required]
        [MaxLength(100)]
        public string Role { get; set; } = null!;
    }
}
