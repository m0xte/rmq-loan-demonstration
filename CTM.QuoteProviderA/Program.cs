using CTM.Contracts;
using CTM.QuoteProviderBase;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace CTM.QuoteProviderA
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
                    Amount = 443,
                    CorrelationId = quoteRequest.CorrelationId,
                    Product = "Provider A Loans: offer 1"
                },
                new QuoteResult
                {
                    Amount = 91,
                    CorrelationId = quoteRequest.CorrelationId,
                    Product = "Provider A Loans: offer 2"
                }

            };
        }

        static void Main()
        {
            var cm = ConnectionMultiplexer.Connect("localhost");
            new Program(cm, "QuoteProviderA", "QuoteResult").Run();
        }
    }

}
