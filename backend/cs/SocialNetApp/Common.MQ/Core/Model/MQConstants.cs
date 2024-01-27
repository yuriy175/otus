using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MQ.Core.Model
{
    internal class MQConstants
    {
        internal const string PostChannelName = "/post/feed/posted";
        internal const string DialogWebsockQueueName = "dialogwebsockqueue";
        internal const string CounterQueueName = "countersqueue";
        internal const string CounterDialogQueueName = "counterdialogqueue";
        //internal const string UnreadDialogMessagesQueueName = "unreaddialogmessagesqueue";
    }

    public enum MQMessageTypes
    {
        NewDialogMessage = 1,
        UpdateUnreadDialogMessages = 2,
        UpdateUnreadDialogMessagesCompensate = 3,
    }
}
