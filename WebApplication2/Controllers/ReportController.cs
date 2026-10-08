using Microsoft.AspNetCore.Mvc;
using MongoSalesApi.Models;
using MongoSalesApi.Services;

namespace MongoSalesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly StoreService _storeService;

        public ReportController(StoreService storeService)
        {
            _storeService = storeService;
        }

        // GET: api/report/sellers
        [HttpGet("sellers")]
        public async Task<ActionResult<List<SellerReportDto>>> GetSellersReport()
        {
            var report = await _storeService.GetSellersReportAsync();
            return Ok(report);
        }

        // POST: api/report/seed
        // Ёндпоинт дл€ генерации начальных данных
        [HttpPost("seed")]
        public async Task<IActionResult> SeedData()
        {
            await _storeService.SeedDataAsync();
            return Ok("Ѕаза данных успешно заполнена тестовыми данными.");
        }
    }
}