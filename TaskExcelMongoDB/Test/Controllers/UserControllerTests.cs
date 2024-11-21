using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskExcelMongoDB.Controllers;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Services.Interfaces;
using Xunit;

namespace TaskExcelMongoDB.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(null!, _mockUserService.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOk_WhenUsersExist()
        {
            // Arrange
            var mockUsers = new List<User>
            {
                new User { 
                    FullName = "John Doe", 
                    Address = "123 Main St", 
                    DateOfBirth = "1990-01-01", 
                    MobileNo = "1234567890" 
                },
                new User
                {
                    FullName = "Lana Del Rey",
                    Address = "nilam nagar, solapur",
                    DateOfBirth = "1998-06-04",
                    MobileNo = "1111111111",
                    Salary = 50000
                }
            };
            
            _mockUserService
                .Setup(service => service.GetAllUsers())
                .ReturnsAsync(new OkObjectResult(mockUsers));

            // Act
            var result = await _userController.GetAllUsers();

            // Assert the type and log the first user
            if (result is OkObjectResult okResult && okResult.Value is List<User> users && users.Any())
            {
                var firstUser = users.First();
                var lastUser = users.Last();
                Console.WriteLine($"First user: FullName={firstUser.FullName}, Address={firstUser.Address}, DateOfBirth={firstUser.DateOfBirth}, MobileNo={firstUser.MobileNo}");
                Console.WriteLine($"Last user: FullName={lastUser.FullName}, Address={lastUser.Address}, DateOfBirth={lastUser.DateOfBirth}, MobileNo={lastUser.MobileNo}");
            }
            else
            {
                Console.WriteLine("No user found");
            }

            // Assert the result
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(mockUsers, okObjectResult.Value);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnNoContent_WhenNoUsersExist()
        {
            // Arrange
            _mockUserService
                .Setup(service => service.GetAllUsers())
                .ReturnsAsync(new NoContentResult());

            // Act
            var result = await _userController.GetAllUsers();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task CreateNewUser_ShouldReturnOk_WhenUserCreated()
        {
            // Arrange
            var newUser = new User
            {
                FullName = "John Doe",
                Address = "123 Main St",
                DateOfBirth = "1990-01-01",
                MobileNo = "1234567890",
                Salary = 50000
            };

            _mockUserService.Setup(service => service.CreateNewUser(It.IsAny<User>()))
                .ReturnsAsync(new OkObjectResult(newUser));

            // Act
            var result = await _userController.CreateNewUser(newUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  
            var returnValue = Assert.IsType<User>(okResult.Value);  
            Assert.Equal(newUser.FullName, returnValue.FullName);  
            Assert.Equal(newUser.Address, returnValue.Address);
        }

        [Fact]
        public async Task CreateNewUser_ShouldReturnBadRequest_WhenUserIsInvalid()
        {
            // Arrange
            var invalidUser = new User(); // Missing required fields
            _mockUserService
                .Setup(service => service.CreateNewUser(It.IsAny<User>()))
                .ReturnsAsync(new BadRequestObjectResult("Invalid user data"));

            // Act
            var result = await _userController.CreateNewUser(invalidUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid user data", badRequestResult.Value);
        }

        [Fact]
        public async Task EditUser_ShouldReturnOk_WhenUserUpdated()
        {
            // Arrange
            var updatedUser = new User
            {
                Id = "1", 
                FullName = "John Doe Updated", 
                Address = "456 Updated St", 
                DateOfBirth = "1990-01-01", 
                MobileNo = "1234567890", 
                Salary = 55000
            };

            // Mocking the service method to return an OkObjectResult with the updated user
            _mockUserService.Setup(service => service.EditUser(It.IsAny<User>(), "1"))
                .ReturnsAsync(new OkObjectResult(updatedUser));

            // Act
            var result = await _userController.EditUser(updatedUser, "1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Verify the responce is OkObjectResult
            var returnedValue = Assert.IsType<User>(okResult.Value); // Verify the returned value is user   
            Assert.Equal(updatedUser, returnedValue); // Check the user match
        }

        [Fact]
        public async Task EditUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var updatedUser = new User
            {
                Id = "1",
                FullName = "Nonexistent User",
                Address = "",
                DateOfBirth = "2000-01-01",
                MobileNo = "1234567890",
                Salary = 0
            };

            _mockUserService
                .Setup(service => service.EditUser(It.IsAny<User>(), "1"))
                .ReturnsAsync(new NotFoundObjectResult("User not found"));

            // Act
            var result = await _userController.EditUser(updatedUser, "1");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", notFoundResult.Value);
        }

        
        [Fact]
        public async Task EditUser_ShouldReturnNotFound_WhenUserNotUpdated()
        {
            // Arrange
            var invalidUser = new User
            {
                Id = "1",
                FullName = "",  
                Address = "",   
                DateOfBirth = "",  
                MobileNo = "",    
                Salary = 0
            };

            // Mock the service to return null, simulating a failure to update
            _mockUserService.Setup(service => service.EditUser(It.IsAny<User>(), "1"))
                .ReturnsAsync(new NotFoundResult());

            // Act
            var result = await _userController.EditUser(invalidUser, "1");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);  // Verify the response is NotFoundResult
            Assert.Equal(404, notFoundResult.StatusCode);  // Ensure the status code is 404
        }
    }
}
