namespace EmployeeManagement.Application.DTOs.Employee
{
    public class EmployeeProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectCode { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
