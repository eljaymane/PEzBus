using PEzBus.EventBus.Events;

namespace PEzbus.EventBus;

    public interface IPEzEventBus
    {
        void Register<T>(T instance);
        void Publish(IPEzEvent @event);
    }

