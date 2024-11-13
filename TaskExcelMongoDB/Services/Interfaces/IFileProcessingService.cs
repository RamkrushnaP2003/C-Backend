using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TaskExcelMongoDB.Models;

namespace TaskExcelMongoDB.Services.Interfaces
{
    public interface IFileProcessingService
    {
        Task<List<User>> ProcessExcelFileAsync(IFormFile file);
    }
}
