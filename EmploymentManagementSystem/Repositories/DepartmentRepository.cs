using EmploymentManagementSystem.Data;
using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploymentManagementSystem.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {

        private readonly EmployeeDBContext _context;

        public DepartmentRepository(EmployeeDBContext context)
        {
            _context = context;
        }

        public async Task<Department?> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentAsync()
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<Department> AddDepartmentAsync(DepartMentDto departmentDto)
        {
            var department = new Department
            {
                DepartmentName = departmentDto.DepartmentName,
                DepartmentDescription = departmentDto.DepartmentDescription,
                DepartmentManagerName = departmentDto.DepartmentManagerName,
                IsActive = departmentDto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            return department;
        }

        public async Task<bool> UpdateDepartmentAsync(int departmentId, DepartMentDto departmentDto)
        {
            var department = await _context.Departments.FindAsync(departmentId);

            if (department == null)
                return false;

            department.DepartmentName = departmentDto.DepartmentName;
            department.DepartmentDescription = departmentDto.DepartmentDescription;
            department.DepartmentManagerName = departmentDto.DepartmentManagerName;
            department.IsActive = departmentDto.IsActive;
            department.UpdatedAt = DateTime.UtcNow;

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            var department = await _context.Departments.FindAsync(departmentId);

            if (department == null)
                return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
