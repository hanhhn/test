using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.test
{
    internal class DeadLetterExchange
    {
        public const string WORK_EXCHANGE_NAME = "WORK_EXCHANGE_NAME";
        public const string WORK_QUEUE_NAME = "WORK_QUEUE_NAME";

        public const string DEAD_LETTER_EXCHANGE = "DEAD_LETTER_EXCHANGE";
        public const string DEAD_LETTER_QUEUE = "DEAD_LETTER_QUEUE";

        public void DeadLetterDeclare()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: DEAD_LETTER_EXCHANGE, type: ExchangeType.Direct);

                Dictionary<string, object> arg = new ();
                arg.Add("x-dead-letter-exchange", WORK_EXCHANGE_NAME); // root point here
                arg.Add("x-message-ttl", 1000);

                channel.QueueDeclare(queue: DEAD_LETTER_QUEUE, durable: false, exclusive: false, autoDelete: false, arguments: arg);

                channel.QueueBind(queue: DEAD_LETTER_QUEUE, exchange: DEAD_LETTER_EXCHANGE, routingKey: "");

                Console.WriteLine($" [x][DeadLetter] {DEAD_LETTER_EXCHANGE} created, {DEAD_LETTER_QUEUE} created");
            }
        }


        public void Send()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: WORK_EXCHANGE_NAME, type: ExchangeType.Fanout);

                string message = string.Format("DeadLetterExchange! {0}", DateTime.Now.ToFileTime());
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: WORK_EXCHANGE_NAME,
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);

                Console.WriteLine(" [x][Worker] Sent [{0}]", message);
            }
        }


        public void Receive()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: WORK_EXCHANGE_NAME, type: ExchangeType.Fanout);

                var queueName = WORK_QUEUE_NAME;
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: queueName, exchange: WORK_EXCHANGE_NAME, routingKey: "");

                Console.WriteLine(" [*][Worker] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x][Worker] Received [{0}]", message);

                    try
                    {
                        Console.WriteLine(" [x][Worker] executing...");
                        throw new NullReferenceException("This is exception");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" [x][Worker] error: {0}", ex.Message);

                        using (var connection = factory.CreateConnection())
                        using (var channel = connection.CreateModel())
                        {
                            channel.BasicPublish(exchange: DEAD_LETTER_EXCHANGE,
                                             routingKey: "",
                                             basicProperties: null,
                                             body: Encoding.UTF8.GetBytes(ex.Message));

                            Console.WriteLine(" [x][Worker] Resent [{0}]", ex.Message);
                        }

                    }
                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            }
        }



    }
}
