using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcapp.Models
{
    
    public class Stock
    {
        public string symbol;
        public int quantity; //Sell trades have negative amount, Buy trades have positive
    }

    public class Trade : Stock
    {
        public decimal unitPrice;
        public DateTime date;
    }

    /// <summary>
    /// Exception class for expressing problems with trades
    /// </summary>
    public class InvalidTradeException : InvalidOperationException {
        public InvalidTradeException(string message) : base(message) { }
    }
}
