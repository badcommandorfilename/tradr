using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcapp.Models
{
    public struct Trade
    {
        public string symbol;
        public DateTime date;
        public decimal unitPrice;
        public uint quantity;
    }

    /// <summary>
    /// Exception class for expressing problems with trades
    /// </summary>
    public class InvalidTradeException : InvalidOperationException {
        public InvalidTradeException(string message) : base(message) { }
    }
}
