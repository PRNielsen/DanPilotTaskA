using DanPilotTaskA.Models;

namespace DanPilotTaskA.Services
{
    public interface IExchangeRatesService
    {
        IEnumerable<ExchangeRate> GetRates();
    }
}
