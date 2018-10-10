using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CTM.QuoteAPI.Model.Impl
{
    /// <summary>
    /// Quote correlation store (for redis!)
    /// </summary>
    public class QuoteStore : IQuoteStore
    {
        IConnectionMultiplexer connectionMultiplexer;
        
        public QuoteStore(IConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        public Guid NewQuoteSession()
        {
            return Guid.NewGuid();
        }

        public bool AddQuoteResult(QuoteResult quoteResult)
        {
            var db = connectionMultiplexer.GetDatabase();

            db.ListLeftPush(
                key: $"session_{quoteResult.CorrelationId}_results",
                value: JsonConvert.SerializeObject(quoteResult));

            return true;
        }

        public List<QuoteResult> GetQuoteResults(Guid guid)
        {
            var db = connectionMultiplexer.GetDatabase();

            return db
                .ListRange(key: $"session_{guid}_results")
                .Select(s => JsonConvert.DeserializeObject<QuoteResult>(s))
                .ToList();
        }
    }
}
