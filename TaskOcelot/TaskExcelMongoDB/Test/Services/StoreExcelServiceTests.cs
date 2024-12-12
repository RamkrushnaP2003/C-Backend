using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Repositories.Interfaces;
using TaskExcelMongoDB.Services.Implementations;
using TaskExcelMongoDB.Services.Interfaces;
using Xunit;

namespace TaskExcelMongoDB.Tests.Services
{
    public class StoreExcelServiceTests
    {
        private readonly Mock<IStoreExcelRepository> _mockStoreExcelRepository;
        private readonly Mock<IFileProcessingService> _mockFileProcessingService;
        private readonly StoreExcelService _storeExcelService;

        public StoreExcelServiceTests()
        {
            _mockStoreExcelRepository = new Mock<IStoreExcelRepository>();
            _mockFileProcessingService = new Mock<IFileProcessingService>();
            _storeExcelService = new StoreExcelService(_mockStoreExcelRepository.Object, _mockFileProcessingService.Object);
        }

        [Fact]
        public async Task UploadExcel_ShouldReturnOk_WhenUsersAreInserted()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
            fileMock.Setup(f => f.Length).Returns(1024); // Non-empty file
            fileMock.Setup(f => f.FileName).Returns("test.xlsx");

            var users = new List<User>
            {
                new User { FullName = "John Doe", MobileNo = "1234567890", Address = "123 Main St", Salary = 50000, DateOfBirth = "1990-01-01" },
                new User { FullName = "Jane Smith", MobileNo = "9876543210", Address = "456 Elm St", Salary = 60000, DateOfBirth = "1985-05-15" }
            };

            _mockFileProcessingService.Setup(s => s.ProcessExcelFileAsync(fileMock.Object)).ReturnsAsync(users);
            _mockStoreExcelRepository.Setup(r => r.InsertUsers(users)).ReturnsAsync(true);

            // Act
            var result = await _storeExcelService.UploadExcel(fileMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<UploadExcelResponse>(okResult.Value);
            Assert.Equal("2 records inserted successfully!", response.Message);
        }

        [Fact]
        public async Task UploadExcel_ShouldReturnBadRequest_WhenFileIsNullOrEmpty()
        {
            // Arrange
            var nullFile = (IFormFile)null!; // Case: File is null
            var emptyFileMock = new Mock<IFormFile>(); // Case: File is empty
            emptyFileMock.Setup(f => f.Length).Returns(0);

            // Act (null file case)
            var nullFileResult = await _storeExcelService.UploadExcel(nullFile);

            // Assert (null file case)
            var nullBadRequestResult = Assert.IsType<BadRequestObjectResult>(nullFileResult);
            Assert.Equal("Please upload a valid Excel file.", nullBadRequestResult.Value);

            // Act (empty file case)
            var emptyFileResult = await _storeExcelService.UploadExcel(emptyFileMock.Object);

            // Assert (empty file case)
            var emptyBadRequestResult = Assert.IsType<BadRequestObjectResult>(emptyFileResult);
            Assert.Equal("Please upload a valid Excel file.", emptyBadRequestResult.Value);
        }

    }
}
