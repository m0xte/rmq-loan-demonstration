using CTM.Contracts;
using CTM.QuoteProviderBase;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace CTM.QuoteProviderB
{
    public class Program : BaseProgram
    {
        public Program(IConnectionMultiplexer connectionMultiplexer, string receiveChannel, string replyChannel) :
                   base(connectionMultiplexer, receiveChannel, replyChannel)
        {
        }
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
            var cm = ConnectionMultiplexer.Connect("localhost");
            new Program(cm, "QuoteProviderB", "QuoteResult").Run();
        }
    }
}
