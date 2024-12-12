using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Repositories.Interfaces;
using TaskExcelMongoDB.Services.Implementations;
using TaskExcelMongoDB.Services.Interfaces;

namespace TaskExcelMongoDB.Tests.Services
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
        public async Task GetAllUsers_ShouldReturnUsers_WhenUsersExist()
        {
            var users = new List<User>
            {
                new User { Id = "1", FullName = "John Doe", Address = "123 Main St", MobileNo = "1234567890", DateOfBirth = "2000-01-01", Salary = 50000 },
                new User { Id = "2", FullName = "Jane Smith", Address = "456 Elm St", MobileNo = "0987654321", DateOfBirth = "1990-01-01", Salary = 60000 }
            };
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).ReturnsAsync(users);

            var result = await _userService.GetAllUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Dictionary<string, object>>(okResult.Value);
            var returnedUsers = Assert.IsType<List<User>>(response["Users"]);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnEmpty_WhenNoUsersExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).ReturnsAsync(new List<User>());

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Dictionary<string, object>>(okResult.Value);
            var returnedUsers = Assert.IsType<List<User>>(response["Users"]);
            Assert.Empty(returnedUsers);
        }

        [Fact]
        public async Task CreateNewUser_ShouldReturnBadRequest_WhenUserIsNull()
        {
            // Arrange
            User newUser = null;

            // Act
            var result = await _userService.CreateNewUser(newUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // Access the Value property of BadRequestObjectResult
            var response = Assert.IsType<Dictionary<string, object>>(badRequestResult.Value);

            Assert.Equal("Fill the user details", response["Message"]);
        }

        [Fact]
        public async Task CreateNewUser_ShouldReturnOk_WhenUserIsValid()
        {
            // Arrange
            var newUser = new User
            {
                Id = "1",
                FullName = "John Doe",
                Address = "123 Main St",
                MobileNo = "1234567890",
                DateOfBirth = "2000-01-01",
                Salary = 50000
            };

            _mockUserRepository
                .Setup(repo => repo.CreateNewUser(It.IsAny<User>()))
                .ReturnsAsync(newUser); 

            // Act
            var result = await _userService.CreateNewUser(newUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Expecting OkObjectResult

            // Access the Value property of OkObjectResult
            var response = Assert.IsType<Dictionary<string, object>>(okResult.Value);

            // Validate the response content
            Assert.Equal(newUser, response["Message"]);
        }
    
        [Fact]
        public async Task EditUser_ShouldReturnBadRequest_WhenUserDetailsAreInvalid()
        {
            // Arrange
            var updatedUser = new User
            {
                Id = "673c6d841a244ae21ec2904d",
                FullName = "",
                Address = "",
                MobileNo = "1234567890",
                DateOfBirth = "2000-01-01",
                Salary = 50000
            };

            // Act
            var result = await _userService.EditUser(updatedUser, "673c6d841a244ae21ec2904d");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Dictionary<string, object>>(badRequestResult.Value);
            Assert.Equal("Enter valid information", response["Message"]);
        }

        [Fact]
        public async Task EditUser_ShouldReturnOk_WhenUserDetailsAreValid()
        {
            // Arrange
            var updatedUser = new User
            {
                Id = "673c2036b8407b6e8c0e3085",
                FullName = "John Doe Updated",
                Address = "123 Updated St",
                MobileNo = "1234567890",
                DateOfBirth = "2000-01-01",
                Salary = 55000
            };
            _mockUserRepository.Setup(repo => repo.EditUser(updatedUser, "1")).ReturnsAsync(updatedUser);

            // Act
            var result = await _userService.EditUser(updatedUser, "1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Dictionary<string, object>>(okResult.Value);
            Assert.Equal("User with id : 1 updated", response["Message"]);
        }
    }
}
