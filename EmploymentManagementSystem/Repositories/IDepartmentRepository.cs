using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Models;

namespace EmploymentManagementSystem.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<Department>> GetAllDepartmentAsync();
        Task<Department> AddDepartmentAsync(DepartMentDto departmentDto);
        Task<bool> UpdateDepartmentAsync(int departmentId, DepartMentDto departmentDto);
        Task<bool> DeleteDepartmentAsync(int departmentId);
    }
}
