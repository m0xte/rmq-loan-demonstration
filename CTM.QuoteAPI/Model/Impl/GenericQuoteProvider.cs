using CTM.Contracts;
using Newtonsoft.Json;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using StackExchange.Redis;

namespace CTM.QuoteAPI.Model.Impl
{
    public class GenericQuoteProvider : IQuoteProvider
    {
        string channelName;
        ITracer tracer;
        IConnectionMultiplexer connectionMultiplexer;

        public GenericQuoteProvider(ITracer tracer, IConnectionMultiplexer connectionMultiplexer, string channelName)
        {
            this.tracer = tracer;
            this.connectionMultiplexer = connectionMultiplexer;
            this.channelName = channelName;
        }

        public void Send(QuoteRequest quoteRequest)
        {
            using (var scope = tracer.BuildSpan("send")
                .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient)
                .WithTag(Tags.Component.Key, "GenericQuoteProvider")
                .StartActive(finishSpanOnDispose: true))
            {
                var db = connectionMultiplexer.GetDatabase();

                tracer.Inject(scope.Span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(quoteRequest.TraceContext));
                
                var json = JsonConvert.SerializeObject(quoteRequest);
                db.ListLeftPush(channelName, json);
            }
        }
    }
}
