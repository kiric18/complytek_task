namespace EmployeeManagement.Application.DTOs.Project
{
    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public int DepartmentId { get; set; }
    }
}
