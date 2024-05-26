using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockPriceApp.Models;
using StockPriceApp.Services;

namespace StockPriceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public HomeController(FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }

            Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);

            Stock stock = new Stock
            {
                Symbol = _tradingOptions.Value.DefaultStockSymbol,
                CurrentPrice = (double)responseDictionary["c"],
                LowestPrice = (double)responseDictionary["l"],
                HighestPrice = (double)responseDictionary["h"],
                OpenPrice = (double)responseDictionary["o"]
            }; 

            return View(stock);
        }
    }
}
