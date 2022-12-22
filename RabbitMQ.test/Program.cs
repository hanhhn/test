// See https://aka.ms/new-console-template for more information
using RabbitMQ.test;

DeadLetterExchange deadLetterExchange = new DeadLetterExchange();
deadLetterExchange.DeadLetterDeclare();

do
{
    deadLetterExchange.Send();
    deadLetterExchange.Receive();

    Thread.Sleep(5000/2+157);
} while (true);