using MySql.Data.MySqlClient;
using TaskJWT.Models;
using TaskJWT.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using TaskJWT.Data;

namespace TaskJWT.Repositories.Implementations
{
    public class UserRepository : IUserWriteRepository, IUserReadRepository
    {
        private readonly DbContext _dbContext;

        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetByUsername(string username)
        {
            User user = null;
            using (MySqlConnection connection = _dbContext.CreateConnection())
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("sp_GetUserByUsername", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("userNameParam", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPasswordHash = reader["PasswordHash"].ToString();
                            user = new User
                            {
                                Id = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                Role = (UserRole)Convert.ToInt32(reader["RoleId"]),
                                PasswordHash = storedPasswordHash
                            };
                        }
                    }
                }
            }
            return user;
        }

        public void CreateUser(string username, string passwordHash, int roleId)
        {
            using (MySqlConnection connection = _dbContext.CreateConnection())
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("sp_CreateUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_Username", username);
                    command.Parameters.AddWithValue("_PasswordHash", passwordHash);
                    command.Parameters.AddWithValue("_RoleId", roleId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // public List<User> GetAllUsers()
        // {
        //     List<User> users = new List<User>();
        //     using (MySqlConnection connection = _dbContext.CreateConnection())
        //     {
        //         connection.Open();
        //         using (MySqlCommand command = new MySqlCommand("sp_GetAllUser", connection))
        //         {
        //             command.CommandType = System.Data.CommandType.StoredProcedure;
        //             using (var reader = command.ExecuteReader())
        //             {
        //                 while (reader.Read())
        //                 {
        //                     var user = new User
        //                     {
        //                         Id = Convert.ToInt32(reader["UserId"]),
        //                         Username = reader["Username"].ToString(),
        //                         Role = (UserRole)Convert.ToInt32(reader["RoleId"]),
        //                         PasswordHash = reader["PasswordHash"].ToString()
        //                     };
        //                     users.Add(user);
        //                 }
        //             }
        //         }
        //     }
        //     return users;
        // }

        // public List<User> GetAllManagers()
        // {
        //     List<User> managers = new List<User>();
        //     using (MySqlConnection connection = _dbContext.CreateConnection())
        //     {
        //         connection.Open();
        //         using (MySqlCommand command = new MySqlCommand("sp_GetManagers", connection))
        //         {
        //             command.CommandType = System.Data.CommandType.StoredProcedure;
        //             using (var reader = command.ExecuteReader())
        //             {
        //                 while (reader.Read())
        //                 {
        //                     var manager = new User
        //                     {
        //                         Id = Convert.ToInt32(reader["UserId"]),
        //                         Username = reader["Username"].ToString(),
        //                         PasswordHash = reader["PasswordHash"].ToString(),
        //                         Role = (UserRole)Convert.ToInt32(reader["RoleId"])
        //                     };
        //                     managers.Add(manager);
        //                 }
        //             }
        //         }
        //     }
        //     return managers;
        // }

        // public List<User> GetAllEmployees()
        // {
        //     List<User> employees = new List<User>();
        //     using (MySqlConnection connection = _dbContext.CreateConnection())
        //     {
        //         connection.Open();
        //         using (MySqlCommand command = new MySqlCommand("sp_GetEmployees", connection))
        //         {
        //             command.CommandType = System.Data.CommandType.StoredProcedure;
        //             using (var reader = command.ExecuteReader())
        //             {
        //                 while (reader.Read())
        //                 {
        //                     var employee = new User
        //                     {
        //                         Id = Convert.ToInt32(reader["UserId"]),
        //                         Username = reader["Username"].ToString(),
        //                         PasswordHash = reader["PasswordHash"].ToString(),
        //                         Role = (UserRole)Convert.ToInt32(reader["RoleId"])
        //                     };
        //                     employees.Add(employee);
        //                 }
        //             }
        //         }
        //     }
        //     return employees;
        // }

        public List<User> GetAllEmployees() {
            return GetUsersByProcedure("sp_GetEmployees");
        }

        public List<User> GetAllManagers() {
            return GetUsersByProcedure("sp_GetManagers");
        }

        public List<User> GetAllUsers() {
            return GetUsersByProcedure("sp_GetAllUser");
        }

        public List<User> GetUsersByProcedure(string procedureName) {
            List<User> users = new List<User>();
            using(MySqlConnection connection = _dbContext.CreateConnection())
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(procedureName, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            User user = new User
                            {
                                Id = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                Role = (UserRole)Convert.ToInt32(reader["RoleId"])
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }
    }
}
