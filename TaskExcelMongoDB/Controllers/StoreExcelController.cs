using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskExcelMongoDB.Services.Interfaces;

namespace TaskExcelMongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreExcelController : ControllerBase
    {
        private readonly IStoreExcelService _storeExcelService;

        public StoreExcelController(IStoreExcelService storeExcelService)
        {
            _storeExcelService = storeExcelService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            return await _storeExcelService.UploadExcel(file);
        }
    }
}
