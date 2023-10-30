using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MQ.Core.Services
{
    public class MQSender
    {
        private const string ChannelName = "/post/feed/posted";
        public void SendPost(uint userId, string post)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: ChannelName, type: ExchangeType.Topic);

            var routingKey = userId.ToString();
            var body = Encoding.UTF8.GetBytes(post);
            channel.BasicPublish(exchange: ChannelName,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
