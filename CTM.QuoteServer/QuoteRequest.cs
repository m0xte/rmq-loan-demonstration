using System;
using System.Collections.Generic;
using System.Text;

namespace CTM.QuoteServer
{
    public class QuoteRequest
    {
        public Guid CorrelationId { get; set; }
        public string Name { get; set; }
    }
}
