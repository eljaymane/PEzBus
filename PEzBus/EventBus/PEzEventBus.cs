using PEzbus.EventBus;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.MethodInvoker;
using PEzBus.EventBus.Repository;

namespace PEzBus.EventBus;
public sealed class PEzEventBus() : IPEzEventBus
{
    private readonly IBusRepository<IPEzEvent,EventPriority> _busRepository = new EventsRepository();

    public void Register<T>(T instance)
    {
       _busRepository.Register<T>(instance);
    }
    public void Publish(IPEzEvent @event,EventPriority priority = EventPriority.HIGH)
    {
        _busRepository.Publish(@event, priority);
    }
}

