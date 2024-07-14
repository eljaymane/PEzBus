using PEzbus.EventBus;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.MethodInvoker;
using PEzBus.EventBus.Repository;

namespace PEzBus.EventBus;
public sealed class PEzEventBus(IPEzMethodInvoker methodInvoker) : IPEzEventBus
{
    private readonly IPEzBusRepository _busRepository = new PEzBusRepository();

    public void Register<T>(T instance)
    {
       _busRepository.Register<T>(instance);
    }

    public void Publish(IPEzEvent @event)
    {
        var entries = _busRepository.GetMatchingReferences(@event);
        
        methodInvoker.InvokeMethods(entries,@event);
        _busRepository.Cleanup();
    }




}

