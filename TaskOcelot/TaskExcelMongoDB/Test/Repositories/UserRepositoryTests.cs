using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MongoDB.Driver;
using TaskExcelMongoDB.Data;
using TaskExcelMongoDB.Exceptions;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Repositories.Implementations;

namespace TaskExcelMongoDB.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly Mock<IMongoDBContext> _mockContext;
        private readonly Mock<IMongoCollection<User>> _mockCollection;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _mockContext = new Mock<IMongoDBContext>();
            _mockCollection = new Mock<IMongoCollection<User>>();
            _mockContext.Setup(c => c.Users).Returns(_mockCollection.Object);
            _userRepository = new UserRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnUserList_WhenDataIsValid()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", FullName = "John Doe", Address = "123 Main St", MobileNo = "1234567890", DateOfBirth = "2000-01-01", Salary = 50000 },
                new User { Id = "2", FullName = "Jane Smith", Address = "456 Elm St", MobileNo = "0987654321", DateOfBirth = "1990-01-01", Salary = 60000 }
            };

            var mockCursor = new Mock<IAsyncCursor<User>>();

            // Simulate MoveNextAsync and Current behavior
            mockCursor.SetupSequence(cursor => cursor.MoveNextAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true)  // First batch has data
                    .ReturnsAsync(false); // No more data

            mockCursor.Setup(cursor => cursor.Current).Returns(users);

            // Simulate FindAsync returning the mock cursor
            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<User>>(),
                    It.IsAny<FindOptions<User>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _userRepository.GetAllUsers();

            // Debugging output
            Console.WriteLine($"Count = {result.Count}");

            // Verify mock interaction
            _mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(), It.IsAny<FindOptions<User>>(), It.IsAny<CancellationToken>()), Times.Once);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John Doe", result[0].FullName);
            Assert.Equal("Jane Smith", result[1].FullName);
        }

        [Fact]
        public async Task GetAllUsers_ShouldThrowException_WhenUserDataIsInvalid()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", FullName = null, Address = "123 Main St", MobileNo = "1234567890", DateOfBirth = "2000-01-01", Salary = 50000 },
                new User { Id = "2", FullName = "Jane Smith", Address = "456 Elm St", MobileNo = "0987654321", DateOfBirth = "1990-01-01", Salary = 60000 }
            };

            var mockCursor = new Mock<IAsyncCursor<User>>();

            // Simulate the cursor behavior
            mockCursor.SetupSequence(cursor => cursor.MoveNextAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true)  
                    .ReturnsAsync(false); 

            mockCursor.Setup(cursor => cursor.Current).Returns(users);

            // Simulate the `FindAsync` method to return the mocked cursor
            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<User>>(), 
                    It.IsAny<FindOptions<User>>(), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserDataException>(() => _userRepository.GetAllUsers());
        }

        [Fact]
        public async Task CreateNewUser_ShouldAddUser_WhenDataIsValid()
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

            // Act
            var result = await _userRepository.CreateNewUser(newUser);

            // Assert
            _mockCollection.Verify(c => c.InsertOneAsync(newUser, null, default), Times.Once);
            Assert.Equal("John Doe", result.FullName);
        }

        [Fact]
        public async Task CreateNewUser_ShouldThrowException_WhenDataIsInvalid()
        {
            // Arrange
            var newUser = new User
            {
                Id = "1",
                FullName = "",
                Address = "123 Main St",
                MobileNo = null,
                DateOfBirth = "2000-01-01",
                Salary = 50000
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserDataException>(() => _userRepository.CreateNewUser(newUser));
        }

        [Fact]
        public async Task EditUser_ShouldUpdateUser_WhenDataIsValid()
        {
            // Arrange
            var updatedUser = new User
            {
                Id = "673d8822d1461327076d38c9",
                FullName = "John Doe Updated",
                Address = "123 Updated St",
                MobileNo = "1234567890",
                DateOfBirth = "2000-01-01",
                Salary = 55000
            };

            // Mock the FindOneAndUpdateAsync method without using the extension method
            _mockCollection
                .Setup(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<User>>(), 
                    It.IsAny<UpdateDefinition<User>>(),  
                    It.IsAny<FindOneAndUpdateOptions<User, User>>(),  
                    It.IsAny<CancellationToken>()  
                ))
                .ReturnsAsync(updatedUser);  

            // Act
            var result = await _userRepository.EditUser(updatedUser, "673d8822d1461327076d38c9");

            // Assert
            Assert.NotNull(result);  
            Assert.Equal("John Doe Updated", result.FullName);  
            Assert.Equal("123 Updated St", result.Address);  
        }

        [Fact]
        public async Task EditUser_ShouldThrowException_WhenDataIsInvalid()
        {
            // Arrange
            var updatedUser = new User
            {
                Id = "1",
                FullName = "",
                Address = null,
                MobileNo = "1234567890",
                DateOfBirth = "2000-01-01",
                Salary = 55000
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserDataException>(() => _userRepository.EditUser(updatedUser, "1"));
        }
    }
}
