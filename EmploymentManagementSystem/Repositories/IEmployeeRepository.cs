using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Models;

namespace EmploymentManagementSystem.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<Employee>> GetAllEmployeeAsync();
        Task<Employee> AddEmployeeAsync(EmployeeDto employeeDto);
        Task<bool> UpdateEmployeeAsync(int employeeId, EmployeeDto employeeDto);
        Task<bool> DeleteEmployeeAsync(int employeeId);

    }
}
