using System.Collections.Concurrent;
using System.Reflection;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Invoker;
using PEzBus.Extensions;
using PEzBus.EventBus.Queue;
using PEzBus.Util;
using StructLinq;

namespace PEzBus.EventBus.Register
{
    public class InstancesRegister : IRegister<IEvent,EventPriority>
    {
        private IEventQueue _eventQueue;

        public ConcurrentDictionary<Type, List<InstanceInfos>> EventsMapping { get; } = new();

        public InstancesRegister() => _eventQueue = new EventQueue(this as IInstancesRegister);

        public void Publish (IEvent @event, EventPriority priority = EventPriority.High)
        {
            _eventQueue.Publish<IEvent>(@event, priority);
        }
        
        public void Register<T>(T instance)
        {
            var methods = instance!.GetType().GetMethodsOfInstance<T>();
            foreach (var method in methods)
            {
                var referenceInfo = InstanceInfos.FromInstance(instance,method);
                referenceInfo.GetHandledEvent(out var handledEvent);
                if (EventsMapping.ContainsKey(handledEvent!))
                    EventsMapping[handledEvent!].Add(referenceInfo);
                else EventsMapping.TryAdd(handledEvent!, [referenceInfo]);
            }
        }
        
        public void Register<T>(IReadOnlyList<T> instances)
        {
            foreach (var instance in instances)
            {
                var methods = instance!.GetType().GetMethodsOfInstance<T>();
                foreach (var method in methods)
                {
                    var referenceInfo = InstanceInfos.FromInstance(instance,method);
                        
                    referenceInfo.GetHandledEvent(out var handledEvent);
                    if (EventsMapping.ContainsKey(handledEvent!))
                        EventsMapping[handledEvent!].Add(referenceInfo);
                    else EventsMapping.TryAdd(handledEvent!, [referenceInfo]);
                }
            }
        }
   
         public IEnumerable <KeyValuePair<MethodInfo, object>> GetValidInstances(IEvent @event, Func<InstanceInfos,bool> predicate)
        {
             return  EventsMapping[@event.GetType()]
                            .ToStructEnumerable()
                            .Where(predicate)
                            .Select(referenceInfo => new KeyValuePair<MethodInfo, object>(
                                referenceInfo.Method,
                                referenceInfo.Target
                            )).ToEnumerable();
        }
    }
}

public struct TypeInfo : IEquatable<TypeInfo>
{
    public Type type;

    public TypeInfo()
    {
        
    }
   
    public override bool Equals(object? obj)
    {
        return obj is TypeInfo other && Equals(other);
    }

    public bool Equals(TypeInfo other)
    {
        return type.FullName == other.type.FullName;
    }

    public override int GetHashCode()
    {
        return type.GetHashCode();
    }
}