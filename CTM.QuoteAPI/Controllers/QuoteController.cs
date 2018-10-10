using CTM.Contracts;
using CTM.QuoteAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CTM.QuoteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : Controller
    {
        IQuoteStore quoteStore;
        IQuoteAggregator quoteAggregator;

        public QuoteController(IQuoteStore quoteStore, IQuoteAggregator quoteAggregator)
        {
            this.quoteStore = quoteStore;
            this.quoteAggregator = quoteAggregator;
        }

        /// <summary>
        /// Create a new quote
        /// </summary>
        /// <param name="request">New quote request</param>
        /// <returns>Correlation ID</returns>
        [HttpPost("new")]
        public ActionResult<NewQuoteResponse> NewQuote(NewQuoteRequest request)
        {
            // Create new quote session
            var correlationId = quoteStore.NewQuoteSession();

            // Translate the quote request into our internal data structure
            var quoteRequest = new QuoteRequest
            {
                CorrelationId = correlationId,
                Name = request.Name
            };

            // Send to aggregator
            quoteAggregator.Aggregate(quoteRequest);

            // Return correlation id
            return new NewQuoteResponse
            {
                CorrelationId = correlationId
            };
        }

        [HttpPost("result")]
        public void AddQuoteResult(QuoteResult quoteResult)
        {
            quoteStore.AddQuoteResult(quoteResult);
        }

        [HttpGet("results/{id}")]
        public ActionResult<IEnumerable<QuoteResult>> GetQuoteResults(Guid id)
        {
            return quoteStore.GetQuoteResults(id);
        }
    }
}