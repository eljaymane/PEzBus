using System.Collections.Concurrent;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Queue;
using PEzBus.EventBus.Register;

namespace PEzBus.EventBus;

public sealed class PEzBus : IEventBus
{
    private readonly IRegister<IEvent,EventPriority> _register = new InstancesRegister();
    public ConcurrentDictionary<Type,List<InstanceInfos>> InstanceRegister => ((InstancesRegister)_register).EventsMapping;

    public void Register<T>(T instance)
    {
       _register.Register(instance);
    }
    
    public void Register<T>(IEnumerable<T> instances)
    {
       _register.Register(instances);
    }
    public void Publish(IEvent @event,EventPriority priority = EventPriority.High)
    {
        _register.Publish(@event, priority);
    }
}

