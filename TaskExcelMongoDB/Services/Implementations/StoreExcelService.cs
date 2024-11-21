using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Repositories.Interfaces;
using TaskExcelMongoDB.Services.Interfaces;

namespace TaskExcelMongoDB.Services.Implementations
{
    public class StoreExcelService : IStoreExcelService
    {
        private readonly IStoreExcelRepository _storeExcelRepository;
        private readonly IFileProcessingService _fileProcessingService;

        public StoreExcelService(IStoreExcelRepository storeExcelRepository, IFileProcessingService fileProcessingService)
        {
            _storeExcelRepository = storeExcelRepository;
            _fileProcessingService = fileProcessingService;
        }

        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new BadRequestObjectResult("Please upload a valid Excel file.");
            }

            var users = await _fileProcessingService.ProcessExcelFileAsync(file);
            if (await _storeExcelRepository.InsertUsers(users))
            {
                return new OkObjectResult(new UploadExcelResponse
                {
                    Message = $"{users.Count} records inserted successfully!"
                });
            }

            return new BadRequestObjectResult("No valid records to insert.");
        }
    }
}
