using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockPriceApp.Models;
using StockPriceApp.Services;

namespace StockPriceApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public TradeController(FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
        }

        [HttpGet]
        [Route("/trade/Detail/{symbol}")]
        public async Task<IActionResult> Detail(string symbol)
        {
            Dictionary<string, object>? responseDictionary = await _finnhubService.GetCompanyProfile(symbol);

            StockProfile stockProfile = new StockProfile
            {
               Country = responseDictionary["country"].ToString(),
                Currency = responseDictionary["currency"].ToString(),
                Exchange = responseDictionary["exchange"].ToString(),
                FinnhubIndustry = responseDictionary["finnhubIndustry"].ToString(),
                Ipo = responseDictionary["ipo"].ToString(),
                Logo = responseDictionary["logo"].ToString(),
                MarketCapitalization = responseDictionary["marketCapitalization"].ToString(),
                Name = responseDictionary["name"].ToString(),
                Phone = responseDictionary["phone"].ToString(),
                ShareOutstanding = responseDictionary["shareOutstanding"].ToString(),
                Ticker = responseDictionary["ticker"].ToString(),
                Weburl = responseDictionary["weburl"].ToString()
            };

            return View(stockProfile);
        }
    }
}
