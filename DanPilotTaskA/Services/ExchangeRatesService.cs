using DanPilotTaskA.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DanPilotTaskA.Services
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        static HttpClient client = new HttpClient();

        public IEnumerable<ExchangeRate> GetRates()
        {
            IEnumerable<ExchangeRate> exchangeRates = DraftExchangeRates();
            return exchangeRates;
        }

        private IEnumerable<ExchangeRate> DraftExchangeRates()
        {

            string jsonBody = GetHttpRequest().Result;
            string rates = GetRatesFromJSON(jsonBody);
            var ratesDict = CreateDictionary(rates);
            IEnumerable<ExchangeRate> exchangeRates = BuildListOfExchangeRates(ratesDict);

            return exchangeRates;
        }

        private static async Task<string> GetHttpRequest()
        {
            HttpResponseMessage response = await client.GetAsync("https://cdn.forexvalutaomregner.dk/api/latest.json");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        private string GetRatesFromJSON(string jsonBody)
        {
            JObject jsonObject = JObject.Parse(jsonBody);
            string rates = "";
            foreach (KeyValuePair<string, JToken> property in jsonObject)
            {
                if (property.Key == "rates")
                {
                    rates = property.Value.ToString();
                }
            }

            return rates;
        }

        private Dictionary<string, double> CreateDictionary(String rates)
        {
            var ratesDict = rates
                .Trim('{', '}')
                .Split(',')
                .Select(x => x.Split(':')) // Split ISO and Rate into key value pairs
                .ToDictionary(x => x[0] // Place ISO and Rate key value pairs into a dictionary
                .Replace("\"", string.Empty) // Replace unnecessary double quotation marks on ISO
                , x => double.Parse(x[1], CultureInfo.InvariantCulture)); // Persists the data in order to keep the full number

            return ratesDict;
        }

        private IEnumerable<ExchangeRate> BuildListOfExchangeRates(Dictionary<string, double> ratesDict)
        {
            IEnumerable<ExchangeRate> exchangeRates = ratesDict.Select(x => new ExchangeRate
            {
                ISO = x.Key.Replace("\r\n", string.Empty).Trim(),
                Rate = x.Value
            }).ToList();

            return exchangeRates;
        }

    }
}
