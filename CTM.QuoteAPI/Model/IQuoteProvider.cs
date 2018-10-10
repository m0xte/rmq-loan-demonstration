using CTM.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTM.QuoteAPI.Model
{
    public interface IQuoteProvider
    {
        /// <summary>
        /// Send to the quote queue
        /// </summary>
        /// <param name="quoteRequest"></param>
        void Send(QuoteRequest quoteRequest);
    }
}
