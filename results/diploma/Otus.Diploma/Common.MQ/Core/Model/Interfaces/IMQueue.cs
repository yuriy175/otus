using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MQ.Core.Model.Interfaces
{
    public interface IMQueue
    {
        void SendRequest<T>(T data);
        void CreateDirectReceiver(Action<byte[]> action);
    }
}
