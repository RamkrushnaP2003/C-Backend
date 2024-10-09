using CsvUploadApi.Models; // Ensure this is the correct namespace
using CsvHelper; // Make sure CsvHelper is installed
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CsvUploadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvController : ControllerBase
    {
        private readonly string connectionString;

        public CsvController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("upload")] // Ensure this matches your Postman URL
        public IActionResult UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<PersonAddressRecord>().ToList();

            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            foreach (var record in records)
            {
                int personId = InsertOrUpdatePerson(connection, record);
                int addressId = InsertOrUpdateAddress(connection, record);
                LinkPersonAddress(connection, personId, addressId);
            }

            return Ok("CSV data processed and stored successfully.");
        }

        private int InsertOrUpdatePerson(MySqlConnection connection, PersonAddressRecord record)
        {
            var existingPersonId = GetExistingPersonId(connection, record.Person_Name, record.Person_Email);
            if (existingPersonId != null)
            {
                return (int)existingPersonId; // Return existing person ID
            }

            using var cmd = new MySqlCommand("INSERT INTO Person (Name, Email, Contact) VALUES (@p_Name, @p_Email, @p_Contact); SELECT LAST_INSERT_ID();", connection);
            cmd.Parameters.AddWithValue("@p_Name", record.Person_Name);
            cmd.Parameters.AddWithValue("@p_Email", record.Person_Email);
            cmd.Parameters.AddWithValue("@p_Contact", record.Person_Contact);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private int InsertOrUpdateAddress(MySqlConnection connection, PersonAddressRecord record)
        {
            var existingAddressId = GetExistingAddressId(connection, record.Street1, record.City, record.ZipCode);
            if (existingAddressId != null)
            {
                return (int)existingAddressId; // Return existing address ID
            }

            using var cmd = new MySqlCommand("INSERT INTO Address (Street1, Street2, City, ZipCode) VALUES (@a_Street1, @a_Street2, @a_City, @a_ZipCode); SELECT LAST_INSERT_ID();", connection);
            cmd.Parameters.AddWithValue("@a_Street1", record.Street1);
            cmd.Parameters.AddWithValue("@a_Street2", record.Street2);
            cmd.Parameters.AddWithValue("@a_City", record.City);
            cmd.Parameters.AddWithValue("@a_ZipCode", record.ZipCode);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private int? GetExistingPersonId(MySqlConnection connection, string name, string email)
        {
            using var cmd = new MySqlCommand("SELECT Id FROM Person WHERE Name = @Name AND Email = @Email", connection);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Email", email);
            return (int?)cmd.ExecuteScalar();
        }

        private int? GetExistingAddressId(MySqlConnection connection, string street1, string city, string zipCode)
        {
            using var cmd = new MySqlCommand("SELECT Id FROM Address WHERE Street1 = @Street1 AND City = @City AND ZipCode = @ZipCode", connection);
            cmd.Parameters.AddWithValue("@Street1", street1);
            cmd.Parameters.AddWithValue("@City", city);
            cmd.Parameters.AddWithValue("@ZipCode", zipCode);
            return (int?)cmd.ExecuteScalar();
        }

        private void LinkPersonAddress(MySqlConnection connection, int personId, int addressId)
        {
            using var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM PersonAddress WHERE PersonId = @PersonId AND AddressId = @AddressId", connection);
            checkCmd.Parameters.AddWithValue("@PersonId", personId);
            checkCmd.Parameters.AddWithValue("@AddressId", addressId);
            var exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;

            if (!exists)
            {
                using var cmd = new MySqlCommand("INSERT INTO PersonAddress (PersonId, AddressId) VALUES (@p_ID, @a_ID);", connection);
                cmd.Parameters.AddWithValue("@p_ID", personId);
                cmd.Parameters.AddWithValue("@a_ID", addressId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
