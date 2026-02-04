using EmploymentManagementSystem.Data;
using EmploymentManagementSystem.DTOS;
using Microsoft.EntityFrameworkCore;

namespace EmploymentManagementSystem.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly EmployeeDBContext _context;

        public ReportRepository(EmployeeDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeReportDto>> GetEmployeeDirectoryAsync()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .OrderBy(e => e.EmployeeCode)
                .ToListAsync();

            return employees.Select(e => new EmployeeReportDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeCode = e.EmployeeCode,
                FullName = $"{e.FirstName} {e.LastName}",
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                DepartmentName = e.Department?.DepartmentName ?? "N/A",
                Position = e.Position,
                Salary = e.Salary,
                JoiningDate = e.JoiningDate,
                Status = e.IsActive ? "Active" : "Inactive"
            }).ToList();
        }

        public async Task<IEnumerable<DepartmentReportDto>> GetDepartmentReportAsync()
        {
            var departments = await _context.Departments
                .Include(d => d.Employees)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return departments.Select(d => new DepartmentReportDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                Description = d.DepartmentDescription,
                ManagerName = d.DepartmentManagerName ?? "N/A",
                TotalEmployees = d.Employees.Count(e => e.IsActive),
                Status = d.IsActive ? "Active" : "Inactive"
            }).ToList();
        }

        public async Task<IEnumerable<AttendanceResponseDTO>> GetAttendanceReportAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(a => a.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.Date <= endDate.Value);

            var attendances = await query
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Employee.EmployeeCode)
                .ToListAsync();

            return attendances.Select(a => new AttendanceResponseDTO
            {
                AttendanceId = a.AttendanceId,
                EmployeeId = a.EmployeeId,
                EmployeeCode = a.Employee?.EmployeeCode ?? "",
                EmployeeName = a.Employee != null ? $"{a.Employee.FirstName} {a.Employee.LastName}" : "",
                DepartmentName = a.Employee?.Department?.DepartmentName ?? "",
                Date = a.Date,
                CheckInTime = a.CheckInTime?.ToString(@"hh\:mm\:ss"),
                CheckOutTime = a.CheckOutTime?.ToString(@"hh\:mm\:ss"),
                Status = a.Status,
                WorkHours = a.WorkHours,
                Remarks = a.Remarks,
                CreatedAt = a.CreatedAt
            }).ToList();
        }

        public async Task<IEnumerable<SalaryReportDto>> GetSalaryReportAsync()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.Salary)
                .ToListAsync();

            return employees.Select(e => new SalaryReportDto
            {
                EmployeeCode = e.EmployeeCode,
                EmployeeName = $"{e.FirstName} {e.LastName}",
                DepartmentName = e.Department?.DepartmentName ?? "N/A",
                Position = e.Position,
                Salary = e.Salary,
                JoiningDate = e.JoiningDate
            }).ToList();
        }
    }
}
