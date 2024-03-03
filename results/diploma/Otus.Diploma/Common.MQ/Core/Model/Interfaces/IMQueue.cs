using Common.MQ.Core.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MQ.Core.Model.Interfaces
{
    public interface IMQueue
    {
        void SendRequest<T>(ulong deviceId, T data);
        //void CreateDirectReceiver(Action<byte[]> action);
        void CreateDirectReceiver(Func<byte[], Task> action);        
    }
}
