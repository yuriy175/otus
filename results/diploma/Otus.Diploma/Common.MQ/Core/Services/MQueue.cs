using Common.MQ.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Model.Types;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Common.MQ.Core.Services
{
    public class MQueue : MQBase, IMQueue
    {
        private readonly IConnection _sendConnection;
        private readonly IModel _sendChannel;

        private IConnection _rcvConnection;
        private IModel _rcvChannel;
        private readonly string _sendQueueName;
        private readonly string _receiveQueueName;
        private readonly object _lock = new object();

        public MQueue(string sendQueueName, string receiveQueueName)
        {
            _sendQueueName = sendQueueName;
            _receiveQueueName = receiveQueueName;
            lock (_lock)
            {
                (_sendConnection, _sendChannel) = CreateMQ();
            }
            _sendChannel.QueueDeclare(queue: _sendQueueName,
                durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        }

        public void SendRequest<T>(T data)
        {
            var text = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(text);
            _sendChannel.BasicPublish(exchange: string.Empty,
                                 routingKey: _sendQueueName,
                                 basicProperties: null,
                                 body: body);
        }

        public void CreateDirectReceiver(Action<byte[]> action)
        {
            var queueName = _receiveQueueName;
            lock (_lock)
            {
                var (connection, channel) = CreateMQ();

                _rcvConnection = connection;
                _rcvChannel = channel;
            }
            _rcvChannel.QueueDeclare(queue: queueName,
                durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(_rcvChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = Encoding.UTF8.GetString(body);
                action(body);
            };
            _rcvChannel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
