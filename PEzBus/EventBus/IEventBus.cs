using PEzBus.EventBus.Events;
using PEzBus.EventBus.Repository;

namespace PEzbus.EventsPubSub;

    public interface IEventBus
    {
        void Register<T>(T instance);
        void Publish(IEvent @event, EventPriority priority);
    }

