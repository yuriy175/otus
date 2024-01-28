using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MQ.Core.Model.Interfaces
{
    public interface IMQSender
    {
        void SendPost(uint userId, string post);
        void SendNewDialogMessage<T>(string queuePostfix, T data);
        void SendUnreadDialogMessageIds(uint userId, bool isIncrement, int[] unreadMsgIds);
        void SendUnreadDialogMessageIdsFailed(uint userId, bool isIncrement, int[] unreadMsgIds);
    }
}
