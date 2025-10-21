using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePrimaryKeyTypesToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjects_Employees_EmployeeId",
                table: "EmployeeProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjects_Projects_ProjectId",
                table: "EmployeeProjects");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeProjects_ProjectId",
                table: "EmployeeProjects");

            // Departments
            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            // Employees
            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NewDepartmentId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.NewGuid());

            // Projects
            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            // EmployeeProjects
            migrationBuilder.AddColumn<Guid>(
                name: "NewEmployeeId",
                table: "EmployeeProjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.NewGuid());

            migrationBuilder.AddColumn<Guid>(
                name: "NewProjectId",
                table: "EmployeeProjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.NewGuid());

            migrationBuilder.Sql(@"
            -- Employees: map DepartmentId
            UPDATE e
            SET NewDepartmentId = d.NewId
            FROM Employees e
            INNER JOIN Departments d ON e.DepartmentId = d.Id;

            -- EmployeeProjects: map EmployeeId and ProjectId
            UPDATE ep
            SET NewEmployeeId = e.NewId
            FROM EmployeeProjects ep
            INNER JOIN Employees e ON ep.EmployeeId = e.Id;

            UPDATE ep
            SET NewProjectId = p.NewId
            FROM EmployeeProjects ep
            INNER JOIN Projects p ON ep.ProjectId = p.Id;
        ");

            migrationBuilder.DropPrimaryKey("PK_Departments", "Departments");
            migrationBuilder.DropPrimaryKey("PK_Employees", "Employees");
            migrationBuilder.DropPrimaryKey("PK_Projects", "Projects");
            migrationBuilder.DropPrimaryKey("PK_EmployeeProjects", "EmployeeProjects");

            migrationBuilder.DropColumn("Id", "Departments");
            migrationBuilder.DropColumn("Id", "Employees");
            migrationBuilder.DropColumn("DepartmentId", "Employees");
            migrationBuilder.DropColumn("Id", "Projects");
            migrationBuilder.DropColumn("EmployeeId", "EmployeeProjects");
            migrationBuilder.DropColumn("ProjectId", "EmployeeProjects");

            migrationBuilder.RenameColumn("NewId", "Departments", "Id");
            migrationBuilder.RenameColumn("NewId", "Employees", "Id");
            migrationBuilder.RenameColumn("NewDepartmentId", "Employees", "DepartmentId");
            migrationBuilder.RenameColumn("NewId", "Projects", "Id");
            migrationBuilder.RenameColumn("NewEmployeeId", "EmployeeProjects", "EmployeeId");
            migrationBuilder.RenameColumn("NewProjectId", "EmployeeProjects", "ProjectId");

            migrationBuilder.AddPrimaryKey("PK_Departments", "Departments", "Id");
            migrationBuilder.AddPrimaryKey("PK_Employees", "Employees", "Id");
            migrationBuilder.AddPrimaryKey("PK_Projects", "Projects", "Id");
            migrationBuilder.AddPrimaryKey("PK_EmployeeProjects", "EmployeeProjects", new[] { "EmployeeId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProjects_ProjectId",
                table: "EmployeeProjects",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjects_Employees_EmployeeId",
                table: "EmployeeProjects",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjects_Projects_ProjectId",
                table: "EmployeeProjects",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
