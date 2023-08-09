using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformServcie.Dtos;
using RabbitMQ.Client;

namespace PlatformServcie.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() {HostName = _configuration["RabbitMQHost"],
                        Port = int.Parse(_configuration["RabbitMQPort"])};

            try{
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("Connected to the message bus");
            }            
            catch(Exception ex)
            {
                Console.WriteLine($"Message not sent: {ex.Message}");
            }
        }

        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
           var message =  JsonSerializer.Serialize(platformPublishDto);

           if(_connection.IsOpen)
           {
            Console.WriteLine("RabbitMQ message is open, sending messages...");
            SendMessages(message);
            
           }
           else{
            Console.WriteLine("RabbitMQ connection is closed, not sending..");
           }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("-->RabbitMQ connection shutdown");
        }

        private void SendMessages(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                            routingKey:"",
                            basicProperties: null,
                            body: body
                        );
            Console.WriteLine($"--> we have sent Message {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed.");
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}