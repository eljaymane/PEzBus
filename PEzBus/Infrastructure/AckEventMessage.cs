using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEzBus.Infrastructure
{
    public sealed class AckEventMessage : IMessage
    {
        public ArraySegment<byte> ToBytes()
        {
            throw new NotImplementedException();
        }
    }
}
