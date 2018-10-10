using CTM.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTM.QuoteAPI.Model
{
    public interface IQuoteAggregator
    {
        /// <summary>
        /// Aggregate quotes
        /// </summary>
        /// <param name="quoteRequest">Quote request</param>
        void Aggregate(QuoteRequest quoteRequest);
    }
}
