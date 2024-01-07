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
        void SendDialogMessage<T>(T data);
    }
}
