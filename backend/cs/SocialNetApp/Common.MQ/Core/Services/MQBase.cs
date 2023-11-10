using Common.MQ.Core.Model;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.MQ.Core.Services
{
    public class MQBase
    {
        private readonly static string _hostName = default!;
        private readonly static string _userName = default!;
        private readonly static string _passwordName = default!;

        static MQBase() 
        {
            var connectionString = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION");
            var splits = Regex.Split(connectionString!, @"[/:@]").Where( s => !string.IsNullOrEmpty(s)).ToList();
            _userName = splits[1];
            _passwordName = splits[2];
            _hostName = splits[3];
        }

        protected (IConnection, IModel) CreateMQ()
        {
            var factory = new ConnectionFactory { HostName = _hostName, UserName = _userName, Password = _passwordName };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: MQConstants.ChannelName, type: ExchangeType.Topic);

            return (connection, channel);
        }
    }
}
