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
    public class MQReceiver : MQueue
    {
        public MQReceiver() : base(MQConstants.ReceiveQueueName, MQConstants.SendQueueName)
        {
        }
    }
}
