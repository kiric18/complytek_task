using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Core.Entities
{
    /// <summary>
    /// Department entity with name and office location.
    /// </summary>
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(200)]
        public string? OfficeLocation { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
