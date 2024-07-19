using System.Collections.Concurrent;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Queue;
using PEzBus.EventBus.Register;
using PEzBus.EventBus.Register.Abstractions;

namespace PEzBus.EventBus;

public sealed class PEzBus : IEventBus
{
    private readonly IInstanceRegister _register;
    public ConcurrentDictionary<Type, List<InstanceInfos>> InstanceRegister => _register.GetAll();

    public PEzBus(IInstanceRegister? register)
    {
        _register = register is null ? new LocalInstanceRegister() : register;
    }

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

