using Common.MQ.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Common.MQ.Core.Services
{
    public class MQReceiver : MQBase, IMQReceiver//, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private string _postQueueName;

        public void CreateReceiver()
        {
            var(connection, channel) = CreateMQ();

            channel.ExchangeDeclare(exchange: MQConstants.PostChannelName, type: ExchangeType.Topic);

            _connection = connection;
            _channel = channel;
            // declare a server-named queue
            _postQueueName = _channel.QueueDeclare().QueueName;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public void ReceivePosts(uint userId, Action<uint, string> action)
        {
            var bindingKey = userId.ToString();
            _channel.QueueBind(queue: _postQueueName,
                              exchange: MQConstants.PostChannelName,
                              routingKey: bindingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                var id = Convert.ToUInt32(routingKey);
                action(id, message);
                Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
            };
            _channel.BasicConsume(queue: _postQueueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void CreateDialogReceiver(Action<byte[]> action)
        {
            var (connection, channel) = CreateMQ();

            _connection = connection;
            _channel = channel;
            _channel.QueueDeclare(queue: MQConstants.DialogQueueName,
                durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = Encoding.UTF8.GetString(body);
                //var routingKey = ea.RoutingKey;
                //var id = Convert.ToUInt32(routingKey);
                //var message = JsonSerializer.Deserialize<T>(text);
                action(body);
                //Console.WriteLine($" [x] Received {message}");
            };
            _channel.BasicConsume(queue: MQConstants.DialogQueueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
