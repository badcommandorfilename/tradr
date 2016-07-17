using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcapp.Models
{
    public class Portfolio
    {
        public Guid GUID { get; private set; }
        public string Name { get; private set; }

        private decimal _balance = 0;

        /// <summary>
        /// Portfolio funds in dollars
        /// </summary>
        public decimal Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Balance", "Balance must be positive");
                    
                }
                _balance = value;
            }
        }

        private IList<Trade> _tradeHistory = new List<Trade>();

        public Portfolio(string name, decimal balance)
        {
            Name = name;
            Balance = balance;
        }

        /// <summary>
        /// Adds shares to this portfolio if quote price is affordable
        /// </summary>
        /// <param name="quote">Stock to be purchased</param>
        /// <param name="buyquantity">Number of shares to buy</param>
        /// <returns></returns>
        public IEnumerable<Trade> Buy(Quote quote, uint buyquantity)
        {
            var totalprice = quote.buy * buyquantity;
            try
            {
                Balance -= totalprice; //Withdraw price from balance
                _tradeHistory.Add(new Trade() { date = DateTime.Now, quantity = (int)buyquantity, symbol = quote.symbol, unitPrice = quote.buy });
                return _tradeHistory;
            }
            catch(ArgumentException ex) when (ex.ParamName == "Balance")
            {
                throw new InvalidTradeException($"Portfolio {GUID} can't afford {buyquantity} shares of {quote.symbol} at {quote.buy}");
            }
        }

        /// <summary>
        /// Sells shares at quote price if portfolio has enough
        /// </summary>
        /// <param name="quote">Stock to be purchased</param>
        /// <param name="sellquantity">Number of shares to sell</param>
        /// <returns></returns>
        public IEnumerable<Trade> Sell(Quote quote, uint sellquantity)
        {
            var owned = from s in OwnedStocks where s.symbol == quote.symbol select s.quantity;

            var totalowned = owned.FirstOrDefault(); //Total number of shares or zero if none are owned

            if (sellquantity > totalowned)
            {
                throw new InvalidTradeException($"Portfolio {GUID} only has {totalowned} shares, but wants to sell {sellquantity}");
            }
            else
            {
                Balance += sellquantity * quote.sell; //Deposit balance from quoted price
                _tradeHistory.Add(new Trade() { date = DateTime.Now, quantity = (int)(-1*sellquantity), symbol = quote.symbol, unitPrice = quote.sell });
                return _tradeHistory;
            }
        }

        public IEnumerable<Stock> OwnedStocks
        {
            get
            {
                //Collect all stocks with the same symbols into groups
                var tradegroups = from t in _tradeHistory
                                  group t by t.symbol into g
                                  select g;

                //Count total owned stocks by reducing trade quantity over each group
                return from g in tradegroups
                       select new Stock
                       {
                           quantity = g.Sum(t => t.quantity), //Aggregate of all stocks in group
                           symbol = g.Key //Group key is stock symbol
                       };
            }
        }
    }

    public static class CurrentPortfolio
    {
        public static Portfolio Shared { get; set; } = null;
    }
}
