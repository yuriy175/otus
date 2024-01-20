using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MQ.Core.Model.Types
{
    public class UnreadCountMessage : MessageHeader
    {
        public bool IsIncrement { get; init; }
        public uint UserId { get; init; }
        public int[] UnreadMessageIds {  get; init; }
    }
}
