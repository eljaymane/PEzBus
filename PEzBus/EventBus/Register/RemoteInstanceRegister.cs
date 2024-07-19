using PEzBus.EventBus.Events;
using PEzBus.EventBus.Queue;
using PEzBus.EventBus.Register.Abstractions;
using PEzBus.Infrastructure.SocketClient;
using PEzBus.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PEzBus.EventBus.Register
{
    public class RemoteInstanceRegister(RegisterConnectionMode connectionMode, CancellationTokenSource cancellationTokenSource) : IInstanceRegister
    {
        private SocketClient _SocketClient;
        private readonly EventQueue<IEvent,EventPriority> _EventQueue;

        private void Connect(string address, int port)
        {
            _SocketClient = new SocketClient(address, port, HandleMessage, cancellationTokenSource);
        }

        public ConcurrentDictionary<Type, List<InstanceInfos>> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<MethodInfo, object>> GetValidInstances(IEvent @event, Func<InstanceInfos, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Publish(IEvent @event, EventPriority priority)
        {
            if(connectionMode== RegisterConnectionMode.Server)
                throw new NotImplementedException();
        }

        public void Register<T>(T instance)
        {
            if(connectionMode == RegisterConnectionMode.Client)
                throw new NotImplementedException();
        }

        public void Register<T>(IReadOnlyList<T> instance)
        {
            throw new NotImplementedException();
        }

        private void HandleMessage(ArraySegment<byte> message)
        {
            throw new NotImplementedException();
        }
    }
}
