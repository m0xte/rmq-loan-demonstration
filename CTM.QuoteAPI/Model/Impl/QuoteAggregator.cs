using CTM.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTM.QuoteAPI.Model.Impl
{
    public class QuoteAggregator : IQuoteAggregator
    {
        IEnumerable<IQuoteProvider> quoteProviders;

        public QuoteAggregator(IEnumerable<IQuoteProvider> quoteProviders)
        {
            this.quoteProviders = quoteProviders;
        }
        public void Aggregate(QuoteRequest quoteRequest)
        {
            foreach(var quoteProvider in quoteProviders)
            {
                quoteProvider.Send(quoteRequest);
            }
        }
    }
}
