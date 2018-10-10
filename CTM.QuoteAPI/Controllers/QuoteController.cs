using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTM.QuoteAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace CTM.QuoteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : Controller
    {
        [HttpPost("new")]
        public ActionResult<NewQuoteResponse> NewQuote(NewQuoteRequest request)
        {
            var correlationId = QuoteStore.NewQuote();

            QuoteEngine.SendQuoteRequest(correlationId);
            QuoteEngine.SendQuoteRequest(correlationId);

            return new NewQuoteResponse { CorrelationId = correlationId, QuoteEngineCount = 2 };
        }

        [HttpPost("result")]
        public void AddQuoteResult(QuoteResult quoteResult)
        {
            QuoteStore.AddQuoteResult(quoteResult);
        }

        [HttpGet("results/{id}")]
        public ActionResult<IEnumerable<QuoteResult>> GetQuoteResults(Guid id)
        {
            return QuoteStore.GetResults(id);
        }
    }
}