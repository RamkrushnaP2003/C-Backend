using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace CRUD.Pages.Customers
{
    public class Edit : PageModel
    {
        [BindProperty]
        public int Id { get; set; }

        [BindProperty, Required(ErrorMessage = "The First Name is required")]
        public string FirstName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Last Name is required")]
        public string LastName { get; set; } = "";

        [BindProperty, Required, EmailAddress]
        public string Email { get; set; } = "";

        [BindProperty, Phone]
        public string? Phone { get; set; }

        [BindProperty]
        public string? Address { get; set; }

        [BindProperty, Required]
        public string Company { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try {
                string connectionString = "server=localhost;database=CRUDops;user=ram;password=ram12345";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlSelectQueryWhereClause = "SELECT * FROM customers WHERE id=@Id";

                    using (MySqlCommand command = new MySqlCommand(sqlSelectQueryWhereClause, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read()) {
                                Id = reader.GetInt32(0); 
                                FirstName = reader.GetString(1);
                                LastName = reader.GetString(2);
                                Email = reader.GetString(3);
                                Phone = reader.GetString(4);
                                Address = reader.GetString(5);
                                Company = reader.GetString(6);
                            } else {
                                Response.Redirect("/customers/index");
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                ErrorMessage = ex.Message;
                return;
            }
        }

        public void OnPost() {
            if(!ModelState.IsValid) {
                Console.WriteLine("Invalid Data");
                return;
            } 
            if(Phone == null) Phone = "";
            if(Address == null) Address = "";

            // Update Customer Details
            try {
                string connectionString = "server=localhost;database=CRUDops;user=ram;password=ram12345";

                using (MySqlConnection connection = new MySqlConnection(connectionString)) 
                {
                    connection.Open();
                    string sqlUpdateQuery = "UPDATE customers SET firstname=@FirstName, lastname=@LastName, email=@Email, phone=@Phone, address=@Address, company=@Company WHERE id=@Id;";

                    using (MySqlCommand command = new MySqlCommand(sqlUpdateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@firstname", FirstName);
                        command.Parameters.AddWithValue("@lastname", LastName);
                        command.Parameters.AddWithValue("@email", Email);
                        command.Parameters.AddWithValue("@phone", Phone);
                        command.Parameters.AddWithValue("@address", Address);
                        command.Parameters.AddWithValue("@company", Company);
                        command.Parameters.AddWithValue("@id", Id);

                        command.ExecuteNonQuery();
                    }
                }
            } catch ( Exception ex) {
                ErrorMessage = ex.Message;
                return;
            }

            Response.Redirect("/customers");
        }        
    }
}