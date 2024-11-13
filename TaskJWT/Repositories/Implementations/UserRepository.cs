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

        public CreateUserModel CreateUser(CreateUserModel newUser)
        {
            using (MySqlConnection connection = _dbContext.CreateConnection())
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("sp_CreateUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_Username", newUser.Username);
                    command.Parameters.AddWithValue("_PasswordHash", newUser.Password);
                    command.Parameters.AddWithValue("_RoleId", newUser.Role);
                    command.ExecuteNonQuery();
                }
            }
            return newUser;
        }

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
