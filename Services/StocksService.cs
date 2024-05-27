using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        /// <summary>
        /// Constructor of StocksService class that executes when a new object is created for the class
        /// </summary>
        public StocksService()
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }

        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            // Validation: buyOrderRequest should not be null
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }

            // Model Validation
            ValidationHelper.ModelValidation(buyOrderRequest);

            // convert buyOrderRequest to BuyOrder object
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            // generate a unique id for the buy order
            buyOrder.BuyOrderID = Guid.NewGuid();

            // add the buy order to the list of buy orders
            _buyOrders.Add(buyOrder);

            // convert the buyOrder object to BuyOrderResponse object
            return buyOrder.ToBuyOrderResponse();
        }

        public SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            // Validation: sellOrderRequest should not be null
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }

            // Model Validation
            ValidationHelper.ModelValidation(sellOrderRequest);

            // convert sellOrderRequest to SellOrder object
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            // generate a unique id for the sell order
            sellOrder.SellOrderID = Guid.NewGuid();

            // add the sell order to the list of sell orders
            _sellOrders.Add(sellOrder);

            // convert the sellOrder object to SellOrderResponse object
            return sellOrder.ToSellOrderResponse();
        }

        public List<BuyOrderResponse> GetBuyOrders()
        {
            //Convert all BuyOrder objects into BuyOrderResponse objects
            return _buyOrders
             .OrderByDescending(temp => temp.DateAndTimeOfOrder)
             .Select(temp => temp.ToBuyOrderResponse()).ToList();
        }

        public List<SellOrderResponse> GetSellOrders()
        {
            return _sellOrders
             .OrderByDescending(temp => temp.DateAndTimeOfOrder)
             .Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
