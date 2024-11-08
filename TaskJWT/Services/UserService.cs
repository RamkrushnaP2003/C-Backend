using System;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using TaskJWT.Data;
using TaskJWT.Models;

namespace TaskJWT.Services
{
    public class UserService : IUserService
    {
        private readonly DbContext _dbContext;

        public UserService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User Authenticate(string username, string password)
        {
            User user = null;

            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();

                using (var command = new MySqlCommand("sp_GetUserByUsername", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add the parameter and set its value
                    command.Parameters.AddWithValue("userNameParam", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPasswordHash = reader["PasswordHash"].ToString();
                            if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash))
                            {
                                user = new User
                                {
                                    Id = Convert.ToInt32(reader["UserId"]),
                                    Username = reader["Username"].ToString(),
                                    Role = (UserRole)Convert.ToInt32(reader["RoleId"])
                                };
                            } 
                        }
                    }
                }
            }
            Console.WriteLine("okay..");

            return user;
        }

        public void CreateUser(string username, string passwordHash, int roleId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();
        
                using (var command = new MySqlCommand("sp_CreateUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_Username", username);
                    command.Parameters.AddWithValue("_PasswordHash", passwordHash);
                    command.Parameters.AddWithValue("_RoleId", roleId);
        
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<User> GetAllManagers()
        {
            List<User> managers = new List<User>();
            
            using(MySqlConnection connection = _dbContext.CreateConnection()) {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_GetManagers", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User manager = new User {
                                Id = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                Role = (UserRole)Convert.ToInt32(reader["RoleId"])
                            };
                            managers.Add(manager);
                        }
                    }
                }
            }

            return managers;
        }

        public List<User> GetAllEmployees() {
            List<User> employees = new List<User>();
            using (MySqlConnection connection = _dbContext.CreateConnection())
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_GetEmployees", connection)) 
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (MySqlDataReader reader = command.ExecuteReader()) 
                    {
                        while(reader.Read()) 
                        {
                            User employee = new User
                            {
                                Id = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                Role = (UserRole)Convert.ToInt32(reader["RoleId"]) 
                            };
                            employees.Add(employee);
                        }
                    }
                }
            }
            return employees;
        }
    }
}
