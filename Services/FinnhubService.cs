using StockPriceApp.ServiceContracts;

namespace StockPriceApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymol)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymol)
        {
            throw new NotImplementedException();
        }
    }
}
