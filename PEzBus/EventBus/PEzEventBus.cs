using PEzbus.EventsPubSub;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Repository;
using PEzBus.EventsPubSub.Repository;

namespace PEzbus.EventsPubSub;

public sealed class EventBus : IEventBus
{
    private readonly IRepository<IEvent,EventPriority> _repository = new EventsRepository();

    public void Register<T>(T instance)
    {
       _repository.Register(instance);
    }
    public void Publish(IEvent @event,EventPriority priority = EventPriority.HIGH)
    {
        _repository.Publish(@event, priority);
    }
}

