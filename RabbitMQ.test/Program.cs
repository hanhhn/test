// See https://aka.ms/new-console-template for more information
using RabbitMQ.test;


do
{
    //HelloWorld helloWorld = new HelloWorld();
    //helloWorld.Send();
    //helloWorld.Receive();
    //Thread.Sleep(1000);

    PublishSubscribe publishSubscribe = new PublishSubscribe();
    publishSubscribe.Send();
    publishSubscribe.Receive1();
    publishSubscribe.Receive2();
    Thread.Sleep(3000);
}
while (true);
