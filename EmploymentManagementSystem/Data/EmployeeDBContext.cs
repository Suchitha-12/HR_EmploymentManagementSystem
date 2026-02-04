using EmploymentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploymentManagementSystem.Data
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options): base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Department> Departments { get; set; }

    }
}
