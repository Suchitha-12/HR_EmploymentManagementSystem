using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Models;
using EmploymentManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmploymentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var departments = await _departmentRepository.GetAllDepartmentAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving departments", error = ex.Message });
            }
        }

        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetDepartmentById(int departmentId)
        {
            try
            {
                var department = await _departmentRepository.GetByDepartmentIdAsync(departmentId);

                if (department == null)
                {
                    return NotFound(new { message = $"Department with ID {departmentId} not found" });
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving department", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartMentDto departmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid department data", errors = ModelState });
                }

                var createdDepartment = await _departmentRepository.AddDepartmentAsync(departmentDto);
                return CreatedAtAction(nameof(GetDepartmentById), new { departmentId = createdDepartment.DepartmentId }, createdDepartment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating department", error = ex.Message });
            }
        }

        [HttpPut("{departmentId}")]
        public async Task<IActionResult> UpdateDepartment(int departmentId, [FromBody] DepartMentDto departmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid department data", errors = ModelState });
                }

                var result = await _departmentRepository.UpdateDepartmentAsync(departmentId, departmentDto);

                if (!result)
                {
                    return NotFound(new { message = $"Department with ID {departmentId} not found" });
                }

                return Ok(new { message = "Department updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating department", error = ex.Message });
            }
        }

        [HttpDelete("{departmentId}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            try
            {
                var result = await _departmentRepository.DeleteDepartmentAsync(departmentId);

                if (!result)
                {
                    return NotFound(new { message = $"Department with ID {departmentId} not found" });
                }

                return Ok(new { message = "Department deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting department", error = ex.Message });
            }
        }
    }
}
