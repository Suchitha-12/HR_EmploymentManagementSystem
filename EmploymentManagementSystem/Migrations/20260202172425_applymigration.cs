using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmploymentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class applymigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Departments",
                newName: "DepartmentName");

            migrationBuilder.RenameColumn(
                name: "ManagerName",
                table: "Departments",
                newName: "DepartmentManagerName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Departments",
                newName: "DepartmentDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepartmentName",
                table: "Departments",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DepartmentManagerName",
                table: "Departments",
                newName: "ManagerName");

            migrationBuilder.RenameColumn(
                name: "DepartmentDescription",
                table: "Departments",
                newName: "Description");
        }
    }
}
