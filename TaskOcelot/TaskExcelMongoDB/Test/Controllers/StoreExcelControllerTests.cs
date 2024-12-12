using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.IO;
using System.Threading.Tasks;
using TaskExcelMongoDB.Controllers;
using TaskExcelMongoDB.Services.Interfaces;
using Xunit;

namespace TaskExcelMongoDB.Tests.Controllers
{
    public class StoreExcelControllerTests
    {
        private readonly Mock<IStoreExcelService> _mockStoreExcelService;
        private readonly StoreExcelController _controller;

        public StoreExcelControllerTests()
        {
            _mockStoreExcelService = new Mock<IStoreExcelService>();
            _controller = new StoreExcelController(_mockStoreExcelService.Object);
        }

        [Fact]
        public async Task UploadExcel_ShouldReturnOk_WhenServiceReturnsOk()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
            fileMock.Setup(f => f.Length).Returns(1024); // Non-empty file
            fileMock.Setup(f => f.FileName).Returns("test.xlsx");

            var expectedResult = new OkObjectResult(new { Message = "2 records inserted successfully!" });
            _mockStoreExcelService
                .Setup(service => service.UploadExcel(fileMock.Object))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UploadExcel(fileMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("2 records inserted successfully!", ((dynamic)okResult.Value).Message);
        }

        [Fact]
        public async Task UploadExcel_ShouldReturnBadRequest_WhenServiceReturnsBadRequest()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
            fileMock.Setup(f => f.Length).Returns(0); // Empty file
            fileMock.Setup(f => f.FileName).Returns("empty.xlsx");

            var expectedResult = new BadRequestObjectResult("Please upload a valid Excel file.");
            _mockStoreExcelService
                .Setup(service => service.UploadExcel(fileMock.Object))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UploadExcel(fileMock.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Please upload a valid Excel file.", badRequestResult.Value);
        }

        [Fact]
        public async Task UploadExcel_ShouldReturnBadRequest_WhenFileIsNull()
        {
            // Arrange
            IFormFile nullFile = null!;
            var expectedResult = new BadRequestObjectResult("Please upload a valid Excel file.");
            _mockStoreExcelService
                .Setup(service => service.UploadExcel(nullFile))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UploadExcel(nullFile);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Please upload a valid Excel file.", badRequestResult.Value);
        }
    }
}
