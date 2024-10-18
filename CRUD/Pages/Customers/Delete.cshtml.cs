using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace CRUD.Pages.Customers
{
    public class Delete : PageModel
    {
        private readonly ILogger<Delete> _logger;

        public Delete(ILogger<Delete> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public void OnPost(int id) {
            deleteCustomer(id);
            Response.Redirect("/customers/index");
        }

        private void deleteCustomer(int id) {
            try {
                string connectionString = "server=localhost;database=CRUDops;user=ram;password=ram12345";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlDeleteQuery = "DELETE FROM customers WHERE id=@id";
                    using (MySqlCommand command = new MySqlCommand(sqlDeleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("Cannot Delete Customer : "+ ex.Message);
            }
        }
    }
}