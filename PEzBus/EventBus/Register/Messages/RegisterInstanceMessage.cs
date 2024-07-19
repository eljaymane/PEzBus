using PEzBus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEzBus.EventBus.Register.Messages
{
    public class RegisterInstanceMessage<T> : IMessage
    {
        public Guid Id { get; set; }
        public WeakReference Instance { get; set; }

        public RegisterInstanceMessage(T instance)
        {
            Id = Guid.NewGuid();
            Instance = new WeakReference(instance,false);
        }

        public ArraySegment<byte> ToBytes()
        {
            throw new NotImplementedException();
        }
    }
}
