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

        public decimal Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                if(value >= 0)
                {
                    _balance = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Balance","Balance must be positive");
                }
            }
        }

        private IList<Trade> _tradeHistory = new List<Trade>();

        public Portfolio(string name, decimal balance)
        {
            Name = name;
            Balance = balance;
        }

        public IEnumerable<Trade> Buy(Quote quote, uint quantity)
        {
            var totalprice = quote.buy * quantity;
            try
            {
                Balance -= totalprice;
                _tradeHistory.Add(new Trade() { date = DateTime.Now, quantity = quantity, symbol = quote.symbol, unitPrice = quote.buy });
                return _tradeHistory;
            }
            catch(ArgumentException ex) when (ex.ParamName == "Balance")
            {
                throw new InvalidTradeException($"Portfolio {GUID} can't afford {quantity} shares of {quote.symbol} at {quote.buy}");
            }
        }
    }

    public static class CurrentPortfolio
    {
        public static Portfolio Shared { get; set; } = null;
    }
}
