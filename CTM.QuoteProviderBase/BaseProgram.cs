using CTM.Contracts;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CTM.QuoteProviderBase
{
    public abstract class BaseProgram
    {
        IConnectionMultiplexer connectionMultiplexer;
        string receiveChannel;
        string replyChannel;

        protected BaseProgram(IConnectionMultiplexer connectionMultiplexer, string receiveChannel, string replyChannel)
        {
            this.connectionMultiplexer = connectionMultiplexer;
            this.receiveChannel = receiveChannel;
            this.replyChannel = replyChannel;
        }
     
        /// <summary>
        /// quote handler
        /// </summary>
        /// <param name="quoteRequest">Quote request</param>
        /// <returns>Quotes</returns>
        protected abstract IEnumerable<QuoteResult> GetQuotes(QuoteRequest quoteRequest);
        
        public void Run()
        { 
            Console.WriteLine($"Servicing quotes in channel {receiveChannel}");
            Console.WriteLine("Waiting for work...");

            var db = connectionMultiplexer.GetDatabase();
            while(true)
            {
                var message = db.ListRightPop(receiveChannel);
                if (message.IsNull)
                {
                    Thread.Sleep(500);
                    continue;
                }

                var request = JsonConvert.DeserializeObject<QuoteRequest>(message);
                Console.WriteLine($"Handling request with correlation ID {request.CorrelationId}");
                var results = GetQuotes(request);
                foreach (var result in results)
                {
                    var jsonResult = JsonConvert.SerializeObject(result);
                    db.ListLeftPush(replyChannel, jsonResult);
                }
            }
        }
    }
}
