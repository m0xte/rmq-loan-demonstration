using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTM.Contracts
{
    public class QuoteResult
    {
        public QuoteResult()
        {
            ResultId = Guid.NewGuid();
        }

        /// <summary>
        /// Unique result ID for de-duplication purposes
        /// </summary>
        public Guid ResultId { get; set; }
        
        /// <summary>
        /// Quote session correlation ID
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Quote amount
        /// </summary>
        public int Amount { get; set; }
    }
}
