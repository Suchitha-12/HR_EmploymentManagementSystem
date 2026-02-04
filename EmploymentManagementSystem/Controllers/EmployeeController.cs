using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Models;
using EmploymentManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmploymentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllEmployeeAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving employees", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByEmployeeIdAsync(id);

                if (employee == null)
                {
                    return NotFound(new { message = $"Employee with ID {id} not found" });
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving employee", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid employee data", errors = ModelState });
                }

                var createdEmployee = await _employeeRepository.AddEmployeeAsync(employeeDto);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.EmployeeId }, createdEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating employee", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto employeeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid employee data", errors = ModelState });
                }

                var result = await _employeeRepository.UpdateEmployeeAsync(id, employeeDto);

                if (!result)
                {
                    return NotFound(new { message = $"Employee with ID {id} not found" });
                }

                return Ok(new { message = "Employee updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating employee", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var result = await _employeeRepository.DeleteEmployeeAsync(id);

                if (!result)
                {
                    return NotFound(new { message = $"Employee with ID {id} not found" });
                }

                return Ok(new { message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting employee", error = ex.Message });
            }
        }
    }
}
