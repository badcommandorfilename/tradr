using mvcapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YahooFinance.NET;

namespace mvcapp.Services
{
    public class QuoteService
    {
        public string Exchange { get; private set; }
        YahooFinanceClient yahooFinance = new YahooFinanceClient();

        public QuoteService(string exchange)
        {
            Exchange = exchange;
        }

        public Quote GetQuote(string symbol)
        {
            var stockCode = yahooFinance.GetYahooStockCode(Exchange, symbol);
            var history = yahooFinance.GetDailyHistoricalPriceData(stockCode, DateTime.Now, DateTime.Now - TimeSpan.FromDays(30));

            var data = from p 
                       in history
                       select new Quote() { symbol = symbol, name = stockCode.ToString(), buy = p.High, sell = p.Low };

            return data.First();
        }

    }
}
