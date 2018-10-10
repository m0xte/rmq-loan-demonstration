﻿using CTM.Contracts;
using CTM.QuoteProviderBase;
using System;
using System.Collections.Generic;

namespace CTM.QuoteProviderA
{
    public class Program : BaseProgram
    {
        protected override string QueueName => "QuoteProviderA";

        protected override IEnumerable<QuoteResult> GetQuotes(QuoteRequest quoteRequest)
        {
            return new List<QuoteResult>
            {
                new QuoteResult
                {
                    Amount = 443,
                    CorrelationId = quoteRequest.CorrelationId,
                    Name = "Provider A Loans: offer 1",
                    Provider = "Provider A"
                },
                new QuoteResult
                {
                    Amount = 91,
                    CorrelationId = quoteRequest.CorrelationId,
                    Name = "Provider A Loans: offer 2",
                    Provider = "Provider A"
                }

            };
        }

        static void Main()
        {
            new Program().Run();
        }
    }

}
