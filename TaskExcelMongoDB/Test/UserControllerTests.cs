using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskExcelMongoDB.Controllers;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Services.Interfaces;
using Xunit;

namespace TaskExcelMongoDB.Tests
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
    }
}
