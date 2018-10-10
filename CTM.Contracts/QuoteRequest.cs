using System;

namespace CTM.Contracts
{
    public class QuoteRequest
    {
        public Guid CorrelationId { get; set; }

        public string Name { get; set; }
    }
}
