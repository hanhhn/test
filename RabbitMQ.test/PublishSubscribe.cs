using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.test
{
    internal class PublishSubscribe
    {
        public void Send()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                string message = string.Format("PublishSubscribe! {0}", DateTime.Now.ToLocalTime());
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "logs",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);

                Console.WriteLine(" [x][PublishSubscribe] Sent {0}", message);
            }
        }

        public void Receive1()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var queueName = "PublishSubscribe-1";
                channel.QueueDeclare(queue: queueName,durable: false,exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

                Console.WriteLine(" [*][PublishSubscribe-1] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x][PublishSubscribe-1] Received {0}", message);
                };
                channel.BasicConsume(queue: queueName, autoAck: true,consumer: consumer);
            }
        }

        public void Receive2()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var queueName = "PublishSubscribe-2";
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

                Console.WriteLine(" [*][PublishSubscribe-2] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x][PublishSubscribe-2] Received {0}", message);
                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            }
        }
    }
}
