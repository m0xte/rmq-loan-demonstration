using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTM.QuoteAPI.Model
{
    public class QuoteSession
    {
        public QuoteSession()
        {
            QuoteResults = new List<QuoteResult>();
        }
        public List<QuoteResult> QuoteResults { get; set; }
    }
}
