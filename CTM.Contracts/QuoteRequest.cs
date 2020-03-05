using System;
using System.Collections.Generic;

namespace CTM.Contracts
{
    public class QuoteRequest
    {
        public QuoteRequest()
        {
            TraceContext = new Dictionary<string, string>();
        }

        public Guid CorrelationId { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> TraceContext { get; set; }
    }
}
