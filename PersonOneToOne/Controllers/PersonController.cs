using CsvHelper;
using CsvPerson.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CsvPerson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvUploadController : ControllerBase
    {
        private readonly string _connectionString = "server=localhost;port=3306;database=Person;user=ram;password=ram12345";

        [HttpPost("upload")]
        public IActionResult UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (!file.ContentType.Equals("text/csv") && !file.FileName.EndsWith(".csv"))
                return BadRequest("File is not a valid CSV.");

            // Process the CSV file
            var records = ReadCsv(file);

            if (records.Count == 0)
                return BadRequest("CSV file contains no valid records.");

            // Insert the data into the MySQL database
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                foreach (var person in records)
                {
                    // Insert the Person and get the generated PersonId
                    int personId = InsertPerson(connection, person);

                    // Insert associated addresses using the PersonId
                    foreach (var address in person.Addresses)
                    {
                        InsertAddress(connection, personId, address);
                    }
                }
                connection.Close();
            }

            return Ok("CSV data inserted into database.");
        }

        // Method to read the CSV file and map to Person objects
        private List<Person> ReadCsv(IFormFile file)
        {
            var records = new List<Person>();
        
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Read the header first
                csv.Read();
                csv.ReadHeader();
        
                // Read the rest of the records
                while (csv.Read())
                {
                    var personName = csv.GetField("PersonName");
                    var personEmail = csv.GetField("PersonEmail");
                    var personContact = csv.GetField("PersonContact");
        
                    var street = csv.GetField("Street");
                    var city = csv.GetField("City");
                    var zipCode = csv.GetField("ZipCode");
        
                    // Check if the person already exists in the list
                    var existingPerson = records.Find(p => p.PersonName == personName && p.PersonEmail == personEmail);
                    if (existingPerson != null)
                    {
                        // Add address to existing person
                        existingPerson.Addresses.Add(new Address { Street = street, City = city, ZipCode = zipCode });
                    }
                    else
                    {
                        // Create a new person
                        var newPerson = new Person
                        {
                            PersonName = personName,
                            PersonEmail = personEmail,
                            PersonContact = personContact,
                            Addresses = new List<Address> { new Address { Street = street, City = city, ZipCode = zipCode } }
                        };
                        records.Add(newPerson);
                    }
                }
            }
        
            return records;
        }

        // Method to insert a person using stored procedure
        private int InsertPerson(MySqlConnection connection, Person person)
        {
            using (var cmd = new MySqlCommand("InsertPerson", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("p_PersonName", person.PersonName);
                cmd.Parameters.AddWithValue("p_PersonEmail", person.PersonEmail);
                cmd.Parameters.AddWithValue("p_PersonContact", person.PersonContact);

                // Create an output parameter for PersonId
                var personIdParam = new MySqlParameter("p_PersonId", MySqlDbType.Int32);
                personIdParam.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(personIdParam);

                cmd.ExecuteNonQuery();

                // Return the generated PersonId
                return (int)personIdParam.Value;
            }
        }

        // Method to insert an address using stored procedure
        private void InsertAddress(MySqlConnection connection, int personId, Address address)
        {
            using (var cmd = new MySqlCommand("InsertAddress", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("p_PersonId", personId);
                cmd.Parameters.AddWithValue("p_Street", address.Street);
                cmd.Parameters.AddWithValue("p_City", address.City);
                cmd.Parameters.AddWithValue("p_ZipCode", address.ZipCode);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
