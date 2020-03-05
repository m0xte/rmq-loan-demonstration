using CTM.Contracts;
using CTM.QuoteProviderBase;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CTM.QuoteProviderA
{
    public class Program : BaseProgram
    {
        public Program(ITracer tracer, IConnectionMultiplexer connectionMultiplexer, ChannelConfig channelConfig) : 
            base(tracer, connectionMultiplexer, channelConfig)
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

        private static IServiceProvider Configure()
        {
            var loggerFactory = new LoggerFactory();

            var chan = new ChannelConfig { ReceiveChannel = "QuoteProviderA", ReplyChannel = "QuoteResult" };

            // set up trace
            var sc = new ServiceCollection()
                .AddSingleton<Program>()
                .AddSingleton(chan)
                .AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"))
                .AddLogging();

            var serviceName = Assembly.GetEntryAssembly().GetName().Name;
            var sampler = new ConstSampler(sample: true);
            ITracer tracer = new Tracer.Builder(serviceName)
                .WithLoggerFactory(loggerFactory)
                .WithSampler(sampler)
                .Build();
            GlobalTracer.Register(tracer);
            sc.AddSingleton(tracer);
            return sc.BuildServiceProvider();
        }

        static void Main()
        {
            var sp = Configure();
            sp.GetService<Program>().Run();
        }
    }

}
