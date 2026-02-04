using EmploymentManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmploymentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        #region Employee Directory Reports

        [HttpGet("employee-directory/pdf")]
        public async Task<IActionResult> GetEmployeeDirectoryPdf()
        {
            try
            {
                var pdfBytes = await _reportService.GenerateEmployeeDirectoryPdfAsync();
                return File(pdfBytes, "application/pdf", $"EmployeeDirectory_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating PDF report", error = ex.Message });
            }
        }

        [HttpGet("employee-directory/excel")]
        public async Task<IActionResult> GetEmployeeDirectoryExcel()
        {
            try
            {
                var excelBytes = await _reportService.GenerateEmployeeDirectoryExcelAsync();
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"EmployeeDirectory_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating Excel report", error = ex.Message });
            }
        }

        #endregion

        #region Department Reports

        [HttpGet("departments/pdf")]
        public async Task<IActionResult> GetDepartmentReportPdf()
        {
            try
            {
                var pdfBytes = await _reportService.GenerateDepartmentReportPdfAsync();
                return File(pdfBytes, "application/pdf", $"DepartmentReport_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating PDF report", error = ex.Message });
            }
        }

        [HttpGet("departments/excel")]
        public async Task<IActionResult> GetDepartmentReportExcel()
        {
            try
            {
                var excelBytes = await _reportService.GenerateDepartmentReportExcelAsync();
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"DepartmentReport_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating Excel report", error = ex.Message });
            }
        }

        #endregion

        #region Attendance Reports

        [HttpGet("attendance/pdf")]
        public async Task<IActionResult> GetAttendanceReportPdf([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var pdfBytes = await _reportService.GenerateAttendanceReportPdfAsync(startDate, endDate);
                return File(pdfBytes, "application/pdf", $"AttendanceReport_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating PDF report", error = ex.Message });
            }
        }

        [HttpGet("attendance/excel")]
        public async Task<IActionResult> GetAttendanceReportExcel([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var excelBytes = await _reportService.GenerateAttendanceReportExcelAsync(startDate, endDate);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"AttendanceReport_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating Excel report", error = ex.Message });
            }
        }

        #endregion

        #region Salary Reports

        [HttpGet("salary/pdf")]
        public async Task<IActionResult> GetSalaryReportPdf()
        {
            try
            {
                var pdfBytes = await _reportService.GenerateSalaryReportPdfAsync();
                return File(pdfBytes, "application/pdf", $"SalaryReport_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating PDF report", error = ex.Message });
            }
        }

        [HttpGet("salary/excel")]
        public async Task<IActionResult> GetSalaryReportExcel()
        {
            try
            {
                var excelBytes = await _reportService.GenerateSalaryReportExcelAsync();
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"SalaryReport_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating Excel report", error = ex.Message });
            }
        }

        #endregion
    }
}
