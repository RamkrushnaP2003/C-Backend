using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PersonOneToMany.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PersonOneToMany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly string _connectionString;

        public PersonController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            // Check if the file is null or empty
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var headerLine = await reader.ReadLineAsync(); // Read the header line
                    if (string.IsNullOrWhiteSpace(headerLine))
                    {
                        return BadRequest("CSV header is missing.");
                    }

                    var headers = headerLine.Split(',');
                    if (headers.Length != 8 || 
                        headers[0] != "PersonId" || 
                        headers[1] != "PersonName" || 
                        headers[2] != "PersonEmail" || 
                        headers[3] != "PersonContact" || 
                        headers[4] != "Street1" || 
                        headers[5] != "Street2" || 
                        headers[6] != "City" || 
                        headers[7] != "ZipCode")
                    {
                        return BadRequest("CSV header is incorrect.");
                    }

                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                    
                        // Skip empty lines
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue; // Skip this iteration and move to the next line
                        }
                    
                        var values = line.Split(',');
                    
                        // Debug: Log the line being processed
                        Console.WriteLine($"Processing Line: {line} | Values Count: {values.Length}");
                    
                        // Check if the line has the correct number of values (8)
                        if (values.Length != 8)
                        {
                            return BadRequest($"CSV format is incorrect for line: '{line}'. Expected 8 values, but got {values.Length}.");
                        }
                    
                        var person = new Person
                        {
                            PersonId = int.Parse(values[0]),
                            PersonName = values[1],
                            PersonEmail = values[2],
                            PersonContact = values[3],
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    Street = values[4],
                                    City = values[6],
                                    ZipCode = values[7]
                                },
                                new Address
                                {
                                    Street = values[5],
                                    City = values[6],
                                    ZipCode = values[7]
                                }
                            }
                        };
                    
                        // Insert person into the database
                        await InsertPersonIntoDatabase(person);
                    }
                    

                    
                }

                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                // Log the error for further investigation
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        private async Task InsertPersonIntoDatabase(Person person)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                foreach (var address in person.Addresses)
                {
                    using (MySqlCommand command = new MySqlCommand("InsertPerson", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@pPersonId", person.PersonId);
                        command.Parameters.AddWithValue("@pPersonName", person.PersonName);
                        command.Parameters.AddWithValue("@pPersonEmail", person.PersonEmail);
                        command.Parameters.AddWithValue("@pPersonContact", person.PersonContact);
                        command.Parameters.AddWithValue("@pStreet", address.Street);
                        command.Parameters.AddWithValue("@pCity", address.City);
                        command.Parameters.AddWithValue("@pZipCode", address.ZipCode);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }
    }
}
