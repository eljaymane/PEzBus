using PEzBus.EventBus.Events;
using PEzBus.EventBus.Repository;

namespace PEzbus.EventBus;

    public interface IPEzEventBus
    {
        void Register<T>(T instance);
        void Publish(IPEzEvent @event, EventPriority priority);
    }

