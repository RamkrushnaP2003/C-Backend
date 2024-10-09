using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PersonManyToMany.Models;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace PersonManyToMany.Controllers
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
                    var headerLine = await reader.ReadLineAsync();
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
                            PersonContact = values[3]
                        };

                        // Insert person into the database
                        await InsertPersonIntoDatabase(person, values[4], values[5], values[6], values[7]);
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

        private async Task InsertPersonIntoDatabase(Person person, string street1, string street2, string city, string zipCode)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Insert the person into the database
                using (MySqlCommand command = new MySqlCommand("InsertPerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pPersonId", person.PersonId);
                    command.Parameters.AddWithValue("@pPersonName", person.PersonName);
                    command.Parameters.AddWithValue("@pPersonEmail", person.PersonEmail);
                    command.Parameters.AddWithValue("@pPersonContact", person.PersonContact);

                    await command.ExecuteNonQueryAsync();
                }

                // Insert the addresses and create relationships
                int addressId1, addressId2;
                using (MySqlCommand command = new MySqlCommand("InsertAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Insert first address
                    command.Parameters.AddWithValue("@p_Street", street1);
                    command.Parameters.AddWithValue("@p_City", city);
                    command.Parameters.AddWithValue("@p_ZipCode", zipCode);
                    command.Parameters.Add("@p_AddressId", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    await command.ExecuteNonQueryAsync();
                    addressId1 = Convert.ToInt32(command.Parameters["@p_AddressId"].Value);
                }

                using (MySqlCommand command = new MySqlCommand("InsertAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Insert second address
                    command.Parameters.AddWithValue("@p_Street", street2);
                    command.Parameters.AddWithValue("@p_City", city);
                    command.Parameters.AddWithValue("@p_ZipCode", zipCode);
                    command.Parameters.Add("@p_AddressId", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    await command.ExecuteNonQueryAsync();
                    addressId2 = Convert.ToInt32(command.Parameters["@p_AddressId"].Value);
                }

                // Insert the relationships into the join table
                using (MySqlCommand command = new MySqlCommand("InsertPersonAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Insert first relationship
                    command.Parameters.AddWithValue("@p_PersonId", person.PersonId);
                    command.Parameters.AddWithValue("@p_AddressId", addressId1);
                    await command.ExecuteNonQueryAsync();
                }

                using (MySqlCommand command = new MySqlCommand("InsertPersonAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Insert second relationship
                    command.Parameters.AddWithValue("@p_PersonId", person.PersonId);
                    command.Parameters.AddWithValue("@p_AddressId", addressId2);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
