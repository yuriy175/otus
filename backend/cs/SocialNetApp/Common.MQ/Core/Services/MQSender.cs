using Common.MQ.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace Common.MQ.Core.Services
{
    public class MQSender : MQBase, IMQSender
    {
        public void SendPost(uint userId, string post)
        {
            var (connection, channel) = CreateMQ();

            var routingKey = userId.ToString();
            var body = Encoding.UTF8.GetBytes(post);
            channel.BasicPublish(exchange: MQConstants.ChannelName,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);

            channel?.Dispose();
            connection?.Dispose();
        }
    }
}
