// Controllers/PersonController.cs
using EFCoreManyToMany.Data;
using EFCoreManyToMany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using CsvHelper;

namespace EFCoreManyToMany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PersonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
        
            if (Path.GetExtension(file.FileName).ToLower() != ".csv")
            {
                return BadRequest("Invalid file format. Please upload a CSV file.");
            }
        
            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture));
                    var records = csv.GetRecords<CsvPersonRecord>().ToList();
        
                    foreach (var record in records)
                    {
                        // Find or create Person
                        var person = await _context.Persons
                            .Include(p => p.PersonAddresses) // Include related entities to avoid issues
                            .FirstOrDefaultAsync(p => p.Email == record.Person_Email);
        
                        if (person == null)
                        {
                            person = new Person
                            {
                                Name = record.Person_Name,
                                Email = record.Person_Email,
                                Contact = record.Person_Contact
                            };
                            _context.Persons.Add(person); // Add new person if not exists
                        }
        
                        // Find or create Address
                        var address = await _context.Addresses
                            .FirstOrDefaultAsync(a => a.Street == record.Street && a.City == record.City && a.ZipCode == record.ZipCode);
        
                        if (address == null)
                        {
                            address = new Address
                            {
                                Street = record.Street,
                                City = record.City,
                                ZipCode = record.ZipCode
                            };
                            _context.Addresses.Add(address); // Add new address if not exists
                        }
        
                        // Add PersonAddress relationship if it doesn't already exist
                        if (!_context.PersonAddresses.Any(pa => pa.PersonId == person.PersonId && pa.AddressId == address.AddressId))
                        {
                            _context.PersonAddresses.Add(new PersonAddress
                            {
                                PersonId = person.PersonId,
                                AddressId = address.AddressId
                            });
                        }
                    }
        
                    await _context.SaveChangesAsync(); // Save changes to the database
                }
        
                // Print stored data after processing
                var storedData = await GetStoredData();
                return Ok(new { message = "File uploaded and processed successfully.", data = storedData });
            }
            catch (CsvHelperException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"CSV Parsing Error: {ex.Message}");
            }
            catch (DbUpdateException dbEx)
            {
                // This will catch issues with the database update
                var innerException = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database update error: {innerException}");
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing the file: {innerException}");
            }
        }
        


        private async Task<object> GetStoredData()
        {
            var persons = await _context.Persons
                .Include(p => p.PersonAddresses)
                .ThenInclude(pa => pa.Address)
                .ToListAsync();

            return persons.Select(p => new
            {
                p.Name,
                p.Email,
                p.Contact,
                Addresses = p.PersonAddresses.Select(pa => new
                {
                    pa.Address.Street,
                    pa.Address.City,
                    pa.Address.ZipCode
                }).ToList()
            }).ToList();
        }

        public class CsvPersonRecord
        {
            public int Person_ID { get; set; }  // Optional: used for additional logic
            public string Person_Name { get; set; }
            public string Person_Email { get; set; }
            public string Person_Contact { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
        }
    }
}
