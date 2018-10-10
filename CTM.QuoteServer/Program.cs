using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace CTM.QuoteServer
{
    class Program
    {
        static string Queue = "quote_queue";
        static List<string> providers = new List<string>()
        {
            "crapcorp loans",
            "mr shark loans",
            "one meeelion percent loans"
        };

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: Queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

                channel.BasicQos(prefetchCount: 0, prefetchSize: 0, global: false);

                Console.WriteLine("Waiting for work...");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, es) =>
                {
                    var body = es.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var request = JsonConvert.DeserializeObject<QuoteRequest>(message);

                    Console.WriteLine("received: " + request.CorrelationId);

                    Thread.Sleep(2000);

                    var quoteResult = new QuoteResult
                    {
                        CorrelationId = request.CorrelationId,
                        Amount = new Random().Next(1000),
                        Name = request.Name,
                        Provider = providers[new Random().Next(providers.Count)]
                    };

                    var result = JsonConvert.SerializeObject(quoteResult);

                    // call back into service
                    var httpClient = new HttpClient();
                    var stringContent = new StringContent(result, Encoding.UTF8, "application/json");
                    var httpResponse = httpClient.PostAsync("http://localhost:1659/api/quote/result", stringContent).Result;
                    if (httpResponse.IsSuccessStatusCode)
                        channel.BasicAck(deliveryTag: es.DeliveryTag, multiple: false);
                    else
                        Console.WriteLine("Service call failed");
                };

                channel.BasicConsume(queue: Queue, autoAck: false, consumer: consumer);
                Console.ReadLine();
            }
        }
    }
}
