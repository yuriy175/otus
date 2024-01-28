using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Common.MQ.Core.Model.Interfaces
{
    public interface IMQReceiver
    {
        void CreateReceiver();
        void ReceivePosts(uint userId, Action<uint, string> action);
        void NewDialogMessageReceiver(string queuePostfix, Action<byte[]> action);
        void CreateUnreadDialogMessagesCountReceiver(Action<byte[]> action);
        void CreateUnreadDialogMessagesCountFailedReceiver(Action<byte[]> action);        
    }
}
