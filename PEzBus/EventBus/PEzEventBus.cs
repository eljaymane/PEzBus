using System.Collections.Concurrent;
using System.Reflection;
using StructLinq;
using PEzBus.Extensions;
using PEzbus;
using PEzBus.Repository;

namespace PEzBus.EventBus;
public class PEzEventBus : IPEzEventBus
{

    private readonly IPEzBusRepository _busRepository = new PEzRepository();
    private readonly IPezMethodInvoker _methodInvoker;

    public PEzEventBus(IPezMethodInvoker methodInvoker)
    {
        _methodInvoker = methodInvoker; 
    }

    public void Register<T>(T instance)
    {
       _busRepository.Register<T>(instance);
    }

    public void Publish(IPEzEvent @event)
    {
        var entries = _busRepository.GetMatchingReferences(@event);
       
        _methodInvoker.InvokeMethods(entries,@event);
        _busRepository.Cleanup();
    }




}

