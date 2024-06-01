using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.Models;
using ServiceContracts;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly IStocksService _stocksService;

        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
        /// <param name="stocksService">Injecting StocksService</param>
        /// <param name="finnhubService">Injecting FinnhubService</param>
        /// <param name="configuration">Injecting IConfiguration</param>
        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration, IStocksService stocksService)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
            _configuration = configuration;
            _stocksService = stocksService;
        }

        [HttpGet]
        [Route("/[controller]/[action]/{symbol}")]
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

        [Route("/[controller]/{symbol}/trading")]
        public async Task<IActionResult> Index(string symbol)
        {
            //reset stock symbol if not exists
            if (string.IsNullOrEmpty(_tradingOptions.Value.DefaultStockSymbol))
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";


            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await _finnhubService.GetCompanyProfile(_tradingOptions.Value.DefaultStockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);


            //create model object
            StockTrade stockTrade = new StockTrade() { 
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                StockName = companyProfileDictionary != null ? Convert.ToString(companyProfileDictionary["name"]) : "",
                Price = stockQuoteDictionary != null ? Convert.ToDouble(stockQuoteDictionary["c"].ToString()) : 0,
                Quantity = 1
            };

            //load data from finnHubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade() { StockSymbol = Convert.ToString(companyProfileDictionary["ticker"]), StockName = Convert.ToString(companyProfileDictionary["name"]), Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()) };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }

        [HttpPost]
        [Route("/[controller]/buy")]
        public async Task<IActionResult> Buy(string symbol, StockTrade stockTrade)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                symbol = _tradingOptions.Value.DefaultStockSymbol;
            }

            // to buy order request
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                StockSymbol = symbol,
                StockName = stockTrade.StockName,
                DateAndTimeOfOrder = DateTime.Now,
                Price = stockTrade.Price,
                Quantity = stockTrade.Quantity
            };

            //validate the model
            if (!ModelState.IsValid)
            {
                return View("Index", stockTrade);
            }

            //send buy order request to service
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            if (buyOrderResponse == null)
            {
                return View("Error");
            }

            ViewBag.Response = buyOrderResponse;
            return RedirectToAction("Index", symbol);



        }

    }
}
