using System.Collections.Concurrent;
using System.Reflection;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Invoker;
using PEzBus.Extensions;
using PEzBus.EventBus.Queue;
using PEzBus.Util;
using StructLinq;
using PEzBus.EventBus.Register.Abstractions;
using PEzBus.Types;

namespace PEzBus.EventBus.Register
{
    public class LocalInstanceRegister : IInstanceRegister
    {
        private readonly ConcurrentPriorityQueue<IEvent, EventPriority> m_Queue = new(new EventsPriorityComparer());
        private ConcurrentDictionary<Type, List<InstanceInfos>> _EventsMapping { get; } = new();

        public ConcurrentDictionary<Type, List<InstanceInfos>> GetAll() => _EventsMapping;

        public void Publish (IEvent @event, EventPriority priority = EventPriority.High)
        {
            m_Queue.TryEnqueue(new KeyValuePair<IEvent, EventPriority>(@event, priority));
        }
        
        public void Register<T>(T instance)
        {
            var methods = instance!.GetType().GetMethodsOfInstance<T>();
            foreach (var method in methods)
            {
                var instanceInfos = InstanceInfos.FromInstance(instance,method);
                instanceInfos.GetHandledEvent(out var handledEvent);
                
                if (!_EventsMapping.TryAdd(handledEvent!, new List<InstanceInfos> { instanceInfos }))
                    _EventsMapping[handledEvent!].Add(instanceInfos);
            }
        }
        
        public void Register<T>(IReadOnlyList<T> instances)
        {
            foreach (var instance in instances)
            {
                var methods = instance!.GetType().GetMethodsOfInstance<T>();
                foreach (var method in methods)
                {
                    var instanceInfos = InstanceInfos.FromInstance(instance,method);
                        
                    instanceInfos.GetHandledEvent(out var handledEvent);
                    if (!_EventsMapping.TryAdd(handledEvent!, new List<InstanceInfos> { instanceInfos }))
                        _EventsMapping[handledEvent!].Add(instanceInfos);
                }
            }
        }
   
         public IEnumerable <KeyValuePair<MethodInfo, object>> GetValidInstances(IEvent @event, Func<InstanceInfos,bool> predicate)
        {
            if(!_EventsMapping.ContainsKey(@event.GetType())) return Enumerable.Empty<KeyValuePair<MethodInfo, object>>();

             return  _EventsMapping[@event.GetType()]
                            .ToStructEnumerable()
                            .Where(predicate)
                            .Select(referenceInfo => new KeyValuePair<MethodInfo, object>(
                                referenceInfo.Method,
                                referenceInfo.Target
                            )).ToEnumerable();
        }
    }

    public class EventsPriorityComparer : IComparer<(EventPriority, int)>
    {
        public int Compare((EventPriority, int) x, (EventPriority, int) y) => (y.Item1 - x.Item1) - (y.Item2 - x.Item2);

    }
}

