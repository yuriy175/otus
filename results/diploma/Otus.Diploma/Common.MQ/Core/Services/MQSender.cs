using Common.MQ.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Model.Types;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Common.MQ.Core.Services
{
    public class MQSender : MQueue
    {
        public MQSender() : base(MQConstants.SendQueueName, MQConstants.ReceiveQueueName)
        {
        }
    }
}
