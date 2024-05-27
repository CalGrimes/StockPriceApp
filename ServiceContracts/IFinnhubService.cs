namespace StocksApp.ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymol);
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymol);
    }
}
