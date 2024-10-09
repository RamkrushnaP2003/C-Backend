using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CsvUploadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<EmployeeController> _logger;
        private const string ErrorFilePath = "/home/ram/CSV_files/Error.csv";

        public EmployeeController(IConfiguration configuration, ILogger<EmployeeController> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    string headerLine = await stream.ReadLineAsync(); // Read header line and ignore it
                    while (!stream.EndOfStream)
                    {
                        var line = await stream.ReadLineAsync();
                        var values = line.Split(',');

                        // Check for sufficient data
                        if (values.Length < 6)
                        {
                            LogError(values, "Error: Data is insufficient.");
                            continue; // Skip rows with insufficient data
                        }

                        // Check for missing values
                        if (HasMissingValues(values))
                        {
                            LogError(values, "Error: Missing data in line.");
                            continue; // Skip this line
                        }

                        // Try to create an Employee object and catch any parsing errors
                        try
                        {
                            var employee = new Employee
                            {
                                EmployeeId = int.Parse(values[0]),
                                EmployeeName = values[1],
                                EmployeeEmail = values[2],
                                EmployeePosition = values[3],
                                EmployeeSalary = decimal.Parse(values[4]),
                                EmployeeDepartment = values[5]
                            };

                            await InsertEmployeeUsingStoredProcedure(employee);
                        }
                        catch (FormatException ex)
                        {
                            LogError(values, $"Error: {ex.Message}"); // Log format issues
                        }
                        catch (Exception ex)
                        {
                            LogError(values, $"Unexpected error: {ex.Message}"); // Log unexpected errors
                        }
                    }
                }

                return Ok("File uploaded and data processed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing the uploaded CSV file.");
                return StatusCode(500, "Internal server error occurred while processing the file.");
            }
        }

        private async Task InsertEmployeeUsingStoredProcedure(Employee employee)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("InsertEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    command.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    command.Parameters.AddWithValue("@EmployeeEmail", employee.EmployeeEmail);
                    command.Parameters.AddWithValue("@EmployeePosition", employee.EmployeePosition);
                    command.Parameters.AddWithValue("@EmployeeSalary", employee.EmployeeSalary);
                    command.Parameters.AddWithValue("@EmployeeDepartment", employee.EmployeeDepartment);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private void LogError(string[] values, string errorMessage)
        {
            // Log to console for quick debugging
            Console.WriteLine($"{errorMessage}: {string.Join(",", values)}");

            // Log to Error.csv with the first column as the error message
            using (var writer = new StreamWriter(ErrorFilePath, true)) // Append to the file
            {
                writer.WriteLine($"{errorMessage},{string.Join(",", values)}");
            }
        }

        private bool HasMissingValues(string[] values)
        {
            return string.IsNullOrWhiteSpace(values[0]) || // EmployeeId
                   string.IsNullOrWhiteSpace(values[1]) || // EmployeeName
                   string.IsNullOrWhiteSpace(values[2]) || // EmployeeEmail
                   string.IsNullOrWhiteSpace(values[3]) || // EmployeePosition
                   string.IsNullOrWhiteSpace(values[4]) || // EmployeeSalary
                   string.IsNullOrWhiteSpace(values[5]);   // EmployeeDepartment
        }
    }
}
