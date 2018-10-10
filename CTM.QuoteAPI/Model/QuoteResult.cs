using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTM.QuoteAPI.Model
{
    public class QuoteResult
    {
        public Guid CorrelationId { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public int Amount { get; set; }
    }
}
