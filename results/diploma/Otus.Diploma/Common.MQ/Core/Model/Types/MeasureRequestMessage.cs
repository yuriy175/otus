using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MQ.Core.Model.Types
{
    public class MeasureRequestMessage : MessageHeader
    {
        public ulong DeviceId { get; init; }
        public bool UseReplica { get; init; }
    }
}
