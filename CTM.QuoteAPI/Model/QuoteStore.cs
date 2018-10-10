using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTM.QuoteAPI.Model
{
    /// <summary>
    /// Quote correlation store (for redis!)
    /// </summary>
    public static class QuoteStore
    {
        static Dictionary<Guid, QuoteSession> store = new Dictionary<Guid, QuoteSession>();
        static object ctx = new object();

        /// <summary>
        /// Add a new quote. Lifetime in redis would be around 2-3 hours potentially or the life of the quote
        /// </summary>
        /// <returns></returns>
        static public Guid NewQuote()
        {
            var guid = Guid.NewGuid();
            var session = new QuoteSession();
            lock (ctx)
            {
                store.Add(guid, session);
            }
            return guid;
        }

        static public bool AddQuoteResult(QuoteResult quoteResult)
        {
            lock (ctx)
            {
                var guid = quoteResult.CorrelationId;
                if (!store.ContainsKey(guid))
                    return false;
                var session = store[guid] as QuoteSession;
                session.QuoteResults.Add(quoteResult);
                return true;
            }
        }

        static public List<QuoteResult> GetResults(Guid guid)
        {
            lock (ctx)
            {
                if (!store.ContainsKey(guid))
                    return new List<QuoteResult>();
                var session = store[guid] as QuoteSession;
                return session.QuoteResults;
            }
        }
    }
}
