using ExchangeRates.Models;
using ExchangeRates.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeRates.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        // GET: api/<HistoryController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ExchangeDataRequest request) => Ok(await _historyService.Get(request));
        
    }
}
