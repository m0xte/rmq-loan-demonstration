using CTM.Contracts;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading;

namespace CTM.QuoteAPI.Model.Impl
{
    public class QuoteResponseReceiver
    {
        IConnectionMultiplexer connectionMultiplexer;
        IQuoteStore quoteStore;
        string receiveChannel;

        public QuoteResponseReceiver(IConnectionMultiplexer connectionMultiplexer, IQuoteStore quoteStore, string receiveChannel)
        {
            this.connectionMultiplexer = connectionMultiplexer;
            this.quoteStore = quoteStore;
            this.receiveChannel = receiveChannel;
        }

        public void StartReceiving()
        {
            new Thread(ReceiveThread).Start();
        }

        private void ReceiveThread()
        {
            var db = connectionMultiplexer.GetDatabase();
            
            while (true)
            {
                var message = db.ListRightPop(receiveChannel);
                if (message.IsNull)
                {
                    Thread.Sleep(500);
                    continue;
                }

                var response = JsonConvert.DeserializeObject<QuoteResult>(message);
                quoteStore.AddQuoteResult(response);
            }
        }
    }
}
