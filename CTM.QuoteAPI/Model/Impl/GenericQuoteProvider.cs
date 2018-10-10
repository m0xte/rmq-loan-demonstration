using CTM.Contracts;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CTM.QuoteAPI.Model.Impl
{
    public class GenericQuoteProvider : IQuoteProvider
    {
        string queueName;

        public GenericQuoteProvider(string queueName)
        {
            this.queueName = queueName;
        }

        public void Send(QuoteRequest quoteRequest)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var json = JsonConvert.SerializeObject(quoteRequest);

                var body = Encoding.UTF8.GetBytes(json);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: properties,
                                     body: body);
            }
        }
    }
}
