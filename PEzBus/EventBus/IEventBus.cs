using System.Collections.Concurrent;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Queue;
using PEzBus.EventBus.Register;

namespace PEzBus.EventBus;

    public interface IEventBus
    {
        void Register<T>(T instance);
        void Publish(IEvent @event, EventPriority priority);
    }

