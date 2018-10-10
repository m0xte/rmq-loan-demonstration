using CTM.Contracts;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace CTM.QuoteProviderBase
{
    public abstract class BaseProgram
    {
        /// <summary>
        /// Queue name
        /// </summary>
        protected abstract string QueueName { get; }
     
        /// <summary>
        /// quote handler
        /// </summary>
        /// <param name="quoteRequest">Quote request</param>
        /// <returns>Quotes</returns>
        protected abstract IEnumerable<QuoteResult> GetQuotes(QuoteRequest quoteRequest);
        
        public void Run()
        { 
            Console.WriteLine($"Servicing quotes in queue {QueueName}");
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                channel.BasicQos(prefetchCount: 0, prefetchSize: 0, global: false);

                Console.WriteLine("Waiting for work...");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, es) =>
                {
                    var body = es.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var request = JsonConvert.DeserializeObject<QuoteRequest>(message);

                    Console.WriteLine($"Handling request with correlation ID {request.CorrelationId}");


                    var results = GetQuotes(request);
            
                    // call back into service per result
                    var httpClient = new HttpClient();

                    foreach (var result in results)
                    {
                        var jsonResult = JsonConvert.SerializeObject(result);

                        var stringContent = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                        var bin = httpClient.PostAsync("http://localhost:1659/api/quote/result", stringContent).Result;
                    }
                    channel.BasicAck(deliveryTag: es.DeliveryTag, multiple: false);
                    Console.WriteLine($"Returned {results.Count()} quotes");
                };

                channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
                Console.ReadLine();
            }
        }

    }
}
