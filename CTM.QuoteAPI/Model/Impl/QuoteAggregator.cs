using CTM.Contracts;
using Prometheus;
using Serilog;
using System.Collections.Generic;

namespace CTM.QuoteAPI.Model.Impl
{
    public class QuoteAggregator : IQuoteAggregator
    {
        private static readonly Counter PromAggregatedCount =
            Metrics.CreateCounter("quote_aggregator_count", "Count of aggregated quotes");

        IEnumerable<IQuoteProvider> quoteProviders;

        public QuoteAggregator(IEnumerable<IQuoteProvider> quoteProviders)
        {
            this.quoteProviders = quoteProviders;
        }
        public void Aggregate(QuoteRequest quoteRequest)
        {
            foreach(var quoteProvider in quoteProviders)
            {
                Log.Information("Aggregating quote");
                quoteProvider.Send(quoteRequest);
                PromAggregatedCount.Inc();
            }
        }
    }
}
