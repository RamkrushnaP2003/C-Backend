// Controllers/EmployeesController.cs
using System.Text.Json;
using EmployeeApi.Models; // Import the Employee model
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient; // Import MySql.Data
using System.Data;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("api/employees")] // Define the base route for this controller
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly string _connectionString;

        public EmployeesController(ILogger<EmployeesController> logger)
        {
            _logger = logger;
            // Update with your MySQL connection string
            _connectionString = "server=localhost;port=3306;database=Employee;user=ram;password=ram12345;";
        }

        // POST: api/employees
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee data is missing.");
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("InsertEmployee", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        cmd.Parameters.AddWithValue("@empId", employee.Id);
                        cmd.Parameters.AddWithValue("@empName", employee.Name);
                        cmd.Parameters.AddWithValue("@empPosition", employee.Position);
                        cmd.Parameters.AddWithValue("@empSalary", employee.Salary);
                        cmd.Parameters.AddWithValue("@empDepartment", employee.Department);

                        // Execute the command
                        cmd.ExecuteNonQuery();
                    }
                }

                // Log and return success message
                string employeeJSON = JsonSerializer.Serialize(employee, new JsonSerializerOptions { WriteIndented = true });
                _logger.LogInformation($"Employee created: {employeeJSON}");

                return Ok(new { Message = "Employee created successfully!", Employee = employee });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating employee.");
                return StatusCode(500, "Internal server error while creating employee.");
            }
        }
    }
}
