using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace CRUD.Pages.Customers
{
    public class Create : PageModel
    {
        [BindProperty, Required(ErrorMessage = "Fisrt Name in required")]
        public string FirstName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; } = "";

        [BindProperty, Required, EmailAddress]
        public string Email { get; set; } = "";

        [BindProperty, Phone]
        public string? Phone { get; set; }

        [BindProperty]
        public string? Address { get; set; }

        [BindProperty, Required(ErrorMessage = "Company is required")]
        public string Company { get; set; } = "";
        
        private readonly ILogger<Create> _logger;

        public Create(ILogger<Create> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public string ErrorMessage { get; set; } = "";

        public void OnPost()
        {
            if(!ModelState.IsValid) {
                Console.WriteLine("Invalid Data");
                return;
            }

            if(Phone == null) Phone = "";
            if(Address == null) Address = "";

            try {
                string connectionString = "server=localhost;database=CRUDops;user=ram;password=ram12345";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlInsertQuery = "INSERT INTO customers " + "(firstname, lastname, email, phone, address, company) VALUES " + "(@firstname, @lastname, @email, @phone, @address, @company);";
                    using (MySqlCommand command = new MySqlCommand(sqlInsertQuery, connection)) 
                    {
                        command.Parameters.AddWithValue("@firstname", FirstName);
                        command.Parameters.AddWithValue("@lastname", LastName);
                        command.Parameters.AddWithValue("@email", Email);
                        command.Parameters.AddWithValue("@phone", Phone);
                        command.Parameters.AddWithValue("@address", Address);
                        command.Parameters.AddWithValue("@company", Company);

                        command.ExecuteNonQuery();
                    }
                }
            } catch(Exception ex) {
                ErrorMessage = ex.Message;
                return;
            }

            Response.Redirect("/customers/index");
        }
    }
}