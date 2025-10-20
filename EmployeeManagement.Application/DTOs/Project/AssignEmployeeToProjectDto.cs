namespace EmployeeManagement.Application.DTOs.Project
{
    public class AssignEmployeeToProjectDto
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
