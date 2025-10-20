using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Core.Entities
{
    /// <summary>
    /// Project entity. Note: Code is unique and generated via RandomStringGenerator external service.
    /// </summary>
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal Budget { get; set; }

        [Required]
        [MaxLength(100)]
        public string Code { get; set; } = null!; // unique project code

        // Department that handles the project (optional)
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}
