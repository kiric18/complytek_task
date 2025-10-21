namespace EmployeeManagement.Application.DTOs.Project
{
    public class AssignEmployeeToProjectDto
    {
        public Guid EmployeeId { get; set; }
        public Guid ProjectId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
