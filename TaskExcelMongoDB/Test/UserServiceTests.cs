using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Services.Implementations;
using TaskExcelMongoDB.Repositories.Interfaces;
using Xunit;

namespace TaskExcelMongoDB.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOk_WhenUsersExist()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", FullName = "John Doe", Address = "123 Street" },
                new User { Id = "2", FullName = "Jane Doe", Address = "456 Avenue" }
            };

            _mockUserRepository.Setup(repo => repo.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Dictionary<string, object>>(okResult.Value);
            Assert.True(response.ContainsKey("Users")); 
            var returnedUsers = Assert.IsType<List<User>>(response["Users"]);
            Assert.Equal(2, returnedUsers.Count);
        }
    }
}
