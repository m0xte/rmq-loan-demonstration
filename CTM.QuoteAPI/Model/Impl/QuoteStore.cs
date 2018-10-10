using System;
using System.Collections.Generic;

namespace CTM.QuoteAPI.Model.Impl
{
    /// <summary>
    /// Quote correlation store (for redis!)
    /// </summary>
    public class QuoteStore : IQuoteStore
    {
        static Dictionary<Guid, QuoteSession> store = new Dictionary<Guid, QuoteSession>();
        static object ctx = new object();

        public Guid NewQuoteSession()
        {
            var guid = Guid.NewGuid();
            var session = new QuoteSession();
            lock (ctx)
            {
                store.Add(guid, session);
            }
            return guid;
        }

        public bool AddQuoteResult(QuoteResult quoteResult)
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

        public List<QuoteResult> GetQuoteResults(Guid guid)
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
