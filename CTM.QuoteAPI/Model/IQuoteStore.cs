using CTM.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTM.QuoteAPI.Model
{
    /// <summary>
    /// Quote state persistence storage
    /// </summary>
    public interface IQuoteStore
    {
        /// <summary>
        /// Start a new quote session
        /// </summary>
        /// <returns>Quote session correlation ID</returns>
        Guid NewQuoteSession();

        /// <summary>
        /// Add a quote result to the quote session
        /// </summary>
        /// <param name="quoteResult">Quote result</param>
        /// <returns>Success if true</returns>
        bool AddQuoteResult(QuoteResult quoteResult);

        /// <summary>
        /// Get quote results for the quote session
        /// </summary>
        /// <param name="correlationId">Quote session correlation ID</param>
        /// <returns>List of quotes</returns>
        List<QuoteResult> GetQuoteResults(Guid correlationId);
    }
}
