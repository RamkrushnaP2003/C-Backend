using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MongoDB.Driver;
using TaskExcelMongoDB.Data;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Repositories.Implementations;
using Xunit;

namespace TaskExcelMongoDB.Tests.Repositories
{
    public class StoreExcelRepositoryTests
    {
        private readonly Mock<IMongoCollection<User>> _mockCollection;
        private readonly Mock<IMongoDBContext> _mockContext;
        private readonly StoreExcelRepository _storeExcelRepository;

        public StoreExcelRepositoryTests()
        {
            // Mock IMongoCollection<User>
            _mockCollection = new Mock<IMongoCollection<User>>();

            // Mock IMongoDBContext
            _mockContext = new Mock<IMongoDBContext>();
            _mockContext.Setup(c => c.Users).Returns(_mockCollection.Object);

            // Initialize the repository with the mocked context
            _storeExcelRepository = new StoreExcelRepository(_mockContext.Object);
        }

        [Fact]
        public async Task InsertUsers_ShouldReturnTrue_WhenUsersAreInserted()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", FullName = "John Doe", Address = "123 Main St", DateOfBirth = "2000-01-01", MobileNo = "1234567890", Salary = 50000 },
                new User { Id = "2", FullName = "Jane Smith", Address = "456 Elm St", DateOfBirth = "1990-01-01", MobileNo = "0987654321", Salary = 60000 }
            };

            // Mock InsertManyAsync behavior
            _mockCollection
                .Setup(c => c.InsertManyAsync(users, null, default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _storeExcelRepository.InsertUsers(users);

            // Assert
            Assert.True(result); // Ensure the method returns true
            _mockCollection.Verify(
                c => c.InsertManyAsync(users, null, default),
                Times.Once); // Verify InsertManyAsync is called exactly once
        }

        [Fact]
        public async Task InsertUsers_ShouldThrowException_WhenUsersListIsNull()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _storeExcelRepository.InsertUsers(null));

            // Assert the exception message
            Assert.Equal("Users list cannot be null or empty.", exception.Message);

            // Verify InsertManyAsync is not called
            _mockCollection.Verify(c => c.InsertManyAsync(It.IsAny<IEnumerable<User>>(), null, default), Times.Never);
        }

        [Fact]
        public async Task InsertUsers_ShouldThrowException_WhenUsersListIsEmpty()
        {
            // Arrange
            var users = new List<User>();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _storeExcelRepository.InsertUsers(users));

            // Assert the exception message
            Assert.Equal("Users list cannot be null or empty.", exception.Message);

            // Verify InsertManyAsync is not called
            _mockCollection.Verify(c => c.InsertManyAsync(It.IsAny<IEnumerable<User>>(), null, default), Times.Never);
        }


        [Fact]
        public async Task InsertUsers_ShouldThrowException_WhenInsertFails()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", FullName = "John Doe", Address = "123 Main St", DateOfBirth = "2000-01-01", MobileNo = "1234567890", Salary = 50000 }
            };

            // Simulate an exception during InsertManyAsync
            _mockCollection
                .Setup(c => c.InsertManyAsync(users, null, default))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _storeExcelRepository.InsertUsers(users));
            _mockCollection.Verify(
                c => c.InsertManyAsync(users, null, default),
                Times.Once); // Verify InsertManyAsync is called exactly once
        }
    }
}
