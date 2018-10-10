using System;
using System.Collections.Generic;
using System.Text;

namespace CTM.QuoteServer
{
    public class QuoteResult
    {
        public Guid CorrelationId { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public int Amount { get; set; }
    }
}
