using Common.MQ.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Model.Types;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Common.MQ.Core.Services
{
    public class MQSender : MQBase, IMQSender
    {
        public void SendNewDialogMessage<T>(string queuePostfix, T data)
        {
            SendDirectMessage($"{MQConstants.DialogWebsockQueueName}_{queuePostfix}", data);
        }

        public void SendUnreadDialogMessageIds(uint userId, bool isIncrement, int[] unreadMsgIds)
        {
            SendDirectMessage(MQConstants.CounterQueueName, 
                new UnreadCountMessage { 
                    MessageType = MQMessageTypes.UpdateUnreadDialogMessages,
                    UserId = userId,
                    IsIncrement = isIncrement,
                    UnreadMessageIds = unreadMsgIds
                });
        }

        public void SendUnreadDialogMessageIdsFailed(uint userId, bool isIncrement, int[] unreadMsgIds)
        {
            SendDirectMessage(MQConstants.CounterDialogQueueName,
                new UnreadCountMessage
                {
                    MessageType = MQMessageTypes.UpdateUnreadDialogMessagesCompensate,
                    UserId = userId,
                    IsIncrement = isIncrement,
                    UnreadMessageIds = unreadMsgIds
                });
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

        private void SendDirectMessage<T>(string queueName, T data)
        {
            var (connection, channel) = CreateMQ();
            channel.QueueDeclare(queue: queueName,
                durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            var text = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(text);
            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);

            channel?.Dispose();
            connection?.Dispose();
        }
    }
}
