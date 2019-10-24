using CTM.Contracts;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CTM.QuoteAPI.Model.Impl
{
    public class GenericQuoteProvider : IQuoteProvider
    {
        string channelName;
        IConnectionMultiplexer connectionMultiplexer;

        public GenericQuoteProvider(IConnectionMultiplexer connectionMultiplexer, string channelName)
        {
            this.connectionMultiplexer = connectionMultiplexer;
            this.channelName = channelName;
        }

        public void Send(QuoteRequest quoteRequest)
        {
            var db = connectionMultiplexer.GetDatabase();
            var json = JsonConvert.SerializeObject(quoteRequest);
            db.ListLeftPush(channelName, json);
        }
    }
}
