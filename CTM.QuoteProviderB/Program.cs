using CTM.Contracts;
using CTM.QuoteProviderBase;
using System;
using System.Collections.Generic;

namespace CTM.QuoteProviderB
{
    public class Program : BaseProgram
    {
        protected override string QueueName => "QuoteProviderB";

        protected override IEnumerable<QuoteResult> GetQuotes(QuoteRequest quoteRequest)
        {
            return new List<QuoteResult>
            {
                new QuoteResult
                {
                    Amount = 1231,
                    CorrelationId = quoteRequest.CorrelationId,
                    Product = "Provider B Loans: offer 1"
                }
            };
        }

        static void Main()
        {
            new Program().Run();
        }
    }
}
