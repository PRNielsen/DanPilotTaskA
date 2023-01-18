using DanPilotTaskA.Models;
using DanPilotTaskA.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanPilotTaskA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        
        private readonly IExchangeRatesService _exchangeRates; 

        public ExchangeRatesController( IExchangeRatesService exchangeRates)
        {
            _exchangeRates = exchangeRates;
        }

        [HttpGet(Name = "GetExchangeRates")]
        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            return _exchangeRates.GetRates();
        }
    }
}