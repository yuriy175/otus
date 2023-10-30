using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MQ.Core.Services
{
    public class MQReceiver
    {
        private const string ChannelName = "/post/feed/posted";
        private IModel _channel;
        private string _queueName;
        private EventingBasicConsumer _consumer;

        public void CreateReceiver()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.ExchangeDeclare(exchange: ChannelName, type: ExchangeType.Topic);
            // declare a server-named queue
            _queueName = _channel.QueueDeclare().QueueName;
            

            Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

            //_consumer = new EventingBasicConsumer(_channel);
            
            //_channel.BasicConsume(queue: _queueName,
            //                     autoAck: true,
            //                     consumer: _consumer);
        }

        public void ReceivePosts(uint userId, Action<uint, string> action)
        {
            var bindingKey = userId.ToString();
            _channel.QueueBind(queue: _queueName,
                              exchange: ChannelName,
                              routingKey: bindingKey);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                var id = Convert.ToUInt32(routingKey);
                if (id == userId)
                {
                    action(id, message);
                }
                Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
            };
            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: _consumer);
        }
    }
}
