using EmploymentManagementSystem.Data;
using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploymentManagementSystem.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDBContext _context;

        public EmployeeRepository(EmployeeDBContext context)
        {
            _context = context;
        }

        public async Task<Employee?> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Attendances)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeeAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

        public async Task<Employee> AddEmployeeAsync(EmployeeDto employeeDto)
        {
            var department = await _context.Departments.FindAsync(employeeDto.DepartmentId);

            var employee = new Employee
            {
                EmployeeCode = employeeDto.EmployeeCode,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                DateOfBirth = employeeDto.DateOfBirth,
                JoiningDate = employeeDto.JoiningDate,
                DepartmentId = employeeDto.DepartmentId,
                DepartmentName = department?.DepartmentName ?? string.Empty,
                Position = employeeDto.Position,
                Salary = employeeDto.Salary,
                IsActive = employeeDto.IsActive,
                Address = employeeDto.Address,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<bool> UpdateEmployeeAsync(int employeeId, EmployeeDto employeeDto)
        {
            var employee = await _context.Employees.FindAsync(employeeId);

            if (employee == null)
                return false;

            var department = await _context.Departments.FindAsync(employeeDto.DepartmentId);

            employee.EmployeeCode = employeeDto.EmployeeCode;
            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Email = employeeDto.Email;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            employee.DateOfBirth = employeeDto.DateOfBirth;
            employee.JoiningDate = employeeDto.JoiningDate;
            employee.DepartmentId = employeeDto.DepartmentId;
            employee.DepartmentName = department?.DepartmentName ?? string.Empty;
            employee.Position = employeeDto.Position;
            employee.Salary = employeeDto.Salary;
            employee.IsActive = employeeDto.IsActive;
            employee.Address = employeeDto.Address;
            employee.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);

            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
