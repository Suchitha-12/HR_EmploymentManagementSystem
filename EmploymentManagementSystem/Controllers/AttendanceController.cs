using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmploymentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendenceRepository _attendanceRepository;

        public AttendanceController(IAttendenceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttendances()
        {
            try
            {
                var attendances = await _attendanceRepository.GetAllAttendanceAsync();
                return Ok(attendances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving attendances", error = ex.Message });
            }
        }

        [HttpGet("{attendanceId}")]
        public async Task<IActionResult> GetAttendanceById(int attendanceId)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByAttendanceIdAsync(attendanceId);

                if (attendance == null)
                {
                    return NotFound(new { message = $"Attendance with ID {attendanceId} not found" });
                }

                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving attendance", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttendance([FromBody] AttendanceDto attendanceDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid attendance data", errors = ModelState });
                }

                var createdAttendance = await _attendanceRepository.AddAttendanceAsync(attendanceDto);
                return CreatedAtAction(nameof(GetAttendanceById),
                    new { attendanceId = createdAttendance.AttendanceId },
                    createdAttendance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating attendance", error = ex.Message });
            }
        }

        [HttpPut("{attendanceId}")]
        public async Task<IActionResult> UpdateAttendance(int attendanceId, [FromBody] AttendanceDto attendanceDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid attendance data", errors = ModelState });
                }

                var result = await _attendanceRepository.UpdateAttendanceAsync(attendanceId, attendanceDto);

                if (!result)
                {
                    return NotFound(new { message = $"Attendance with ID {attendanceId} not found" });
                }

                return Ok(new { message = "Attendance updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating attendance", error = ex.Message });
            }
        }

        [HttpDelete("{attendanceId}")]
        public async Task<IActionResult> DeleteAttendance(int attendanceId)
        {
            try
            {
                var result = await _attendanceRepository.DeleteAttendanceAsync(attendanceId);

                if (!result)
                {
                    return NotFound(new { message = $"Attendance with ID {attendanceId} not found" });
                }

                return Ok(new { message = "Attendance deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting attendance", error = ex.Message });
            }
        }
    }
}
