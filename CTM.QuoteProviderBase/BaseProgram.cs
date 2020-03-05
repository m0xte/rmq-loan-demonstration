using CTM.Contracts;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Util;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace CTM.QuoteProviderBase
{
    public abstract class BaseProgram
    {
        IConnectionMultiplexer connectionMultiplexer;
        ChannelConfig channelConfig;
        ITracer tracer;

        protected BaseProgram(ITracer tracer, IConnectionMultiplexer connectionMultiplexer, ChannelConfig channelConfig)
        {
            this.connectionMultiplexer = connectionMultiplexer;
            this.channelConfig = channelConfig;
            this.tracer = tracer;
        }
     
        /// <summary>
        /// quote handler
        /// </summary>
        /// <param name="quoteRequest">Quote request</param>
        /// <returns>Quotes</returns>
        protected abstract IEnumerable<QuoteResult> GetQuotes(QuoteRequest quoteRequest);
        
        public void Run()
        {
            Console.WriteLine($"Servicing quotes in channel {channelConfig.ReceiveChannel}");
            Console.WriteLine("Waiting for work...");

            var db = connectionMultiplexer.GetDatabase();
            while (true)
            {
                var message = db.ListRightPop(channelConfig.ReceiveChannel);
                if (message.IsNull)
                {
                    Thread.Sleep(50);
                    continue;
                }

                var request = JsonConvert.DeserializeObject<QuoteRequest>(message);
                var traceContext = tracer.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(request.TraceContext));
                using (var scope = tracer.BuildSpan("receive")
                    .AddReference(References.FollowsFrom, traceContext)
                    .StartActive(finishSpanOnDispose: true))
                {
                    
                    Console.WriteLine($"Handling request with correlation ID {request.CorrelationId}");
                    var results = GetQuotes(request);
                    foreach (var result in results)
                    {
                        using (var scopeInner = tracer.BuildSpan("reply")
                            .StartActive(finishSpanOnDispose: true))
                        {
                            var jsonResult = JsonConvert.SerializeObject(result);
                            db.ListLeftPush(channelConfig.ReplyChannel, jsonResult);
                        }
                    }
                }
            }
        }

    }
}
