using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Core.Entities
{
    /// <summary>
    /// Employee entity representing a staff member. Each employee belongs to one department and can be assigned to many projects.
    /// </summary>
    public class Employee
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;


        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;


        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;


        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }


        // Foreign key to Department
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }


        // Navigation property for many-to-many relation with Project
        public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}
