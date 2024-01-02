using Common.MQ.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Common.MQ.Core.Services
{
    public class MQSender : MQBase, IMQSender
    {
        public void SendDialogMessage<T>(T data)
        {
            var (connection, channel) = CreateMQ();
            channel.QueueDeclare(queue: MQConstants.DialogQueueName,
                durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            //var routingKey = userId.ToString();
            var text = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(text);
            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: MQConstants.DialogQueueName,
                                 basicProperties: null,
                                 body: body);

            channel?.Dispose();
            connection?.Dispose();
        }

        public void SendPost(uint userId, string post)
        {
            var (connection, channel) = CreateMQ();

            channel.ExchangeDeclare(exchange: MQConstants.PostChannelName, type: ExchangeType.Topic);

            var routingKey = userId.ToString();
            var body = Encoding.UTF8.GetBytes(post);
            channel.BasicPublish(exchange: MQConstants.PostChannelName,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);

            channel?.Dispose();
            connection?.Dispose();
        }
    }
}
