using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskExcelMongoDB.Services.Interfaces
{
    public interface IStoreExcelService
    {
        Task<IActionResult> UploadExcel(IFormFile file);
    }
}
