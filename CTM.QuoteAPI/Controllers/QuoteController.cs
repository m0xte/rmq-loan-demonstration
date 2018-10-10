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

        public QuoteController(IQuoteStore quoteStore)
        {
            this.quoteStore = quoteStore;
        }

        [HttpPost("new")]
        public ActionResult<NewQuoteResponse> NewQuote(NewQuoteRequest request)
        {
            var correlationId = quoteStore.NewQuoteSession();

            QuoteEngine.SendQuoteRequest(correlationId);
            QuoteEngine.SendQuoteRequest(correlationId);

            return new NewQuoteResponse { CorrelationId = correlationId, QuoteEngineCount = 2 };
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