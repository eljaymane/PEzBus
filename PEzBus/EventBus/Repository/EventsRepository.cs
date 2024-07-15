using System.Collections.Concurrent;
using System.Reflection;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.MethodInvoker;
using PEzBus.EventBus.Repository;
using PEzBus.Extensions;
using PEzBus.Types;
using PEzBus.Util;
using StructLinq;

namespace PEzBus.EventsPubSub.Repository
{
    public class EventsRepository : IRepository<IEvent,EventPriority>
    {
        private readonly ConcurrentPriorityQueue<IEvent, EventPriority> _eventsQueue = new(new EventsPriorityComparer());
        private readonly ConcurrentDictionary<Type,List<EventsInfos>> _eventsInstanceMapping = new();
        private Thread _invokerThread;
        private IMethodInvoker _methodInvoker = new EventBus.MethodInvoker.MethodInvoker();
        public EventsRepository()
        {
            _invokerThread = BackgroundThread.Start(HandleEvents);
        }
        private void HandleEvents()
        {
            while (true)
            {
                if(_eventsQueue.TryDequeue(out var item))
                    HandleEvent(item.Key);
            }
        }

        private void HandleEvent(IEvent @event)
        {
             var entries = GetMatchingMethodsAndTargets(@event);
             _methodInvoker.InvokeMethods(entries,@event);
        }

        public void Register<T>(T instance)
        {
            var methods = instance!.GetType().GetMethodsOfInstance<T>();
            foreach (var method in methods)
            {
                var referenceInfo = EventsInfos.FromInstance(instance,method);
                referenceInfo.GetHandledEvent(out var handledEvent);
                if (_eventsInstanceMapping.ContainsKey(handledEvent!))
                    _eventsInstanceMapping[handledEvent!].Add(referenceInfo);
                else _eventsInstanceMapping.TryAdd(handledEvent!, [referenceInfo]);
            }
        }
        public void Register<T>(IReadOnlyList<T> instances)
        {
            foreach (var instance in instances)
            {
                var methods = instance!.GetType().GetMethodsOfInstance<T>();
                foreach (var method in methods)
                {
                    var referenceInfo = EventsInfos.FromInstance(instance,method);
                        
                    referenceInfo.GetHandledEvent(out var handledEvent);
                    if (_eventsInstanceMapping.ContainsKey(handledEvent!))
                        _eventsInstanceMapping[handledEvent!].Add(referenceInfo);
                    else _eventsInstanceMapping.TryAdd(handledEvent!, [referenceInfo]);
                }
            }
        }
        
        public void Publish (IEvent @event, EventPriority priority = EventPriority.HIGH)
        {
            _eventsQueue.TryEnqueue(new KeyValuePair<IEvent, EventPriority>(@event, priority));
        }

         public IEnumerable <KeyValuePair<MethodInfo, object>> GetValidInstances(IEvent @event, Func<EventsInfos,bool> predicate)
        {
             return  _eventsInstanceMapping[@event.GetType()]
                            .ToStructEnumerable()
                            .Where(predicate)
                            .Select(referenceInfo => new KeyValuePair<MethodInfo, object>(
                                referenceInfo.Method,
                                referenceInfo.Target
                            )).ToEnumerable();
        }

        public IEnumerable<KeyValuePair<MethodInfo,object>> GetMatchingMethodsAndTargets(IEvent @event)
        {
            var eventInstances = GetValidInstances(@event, x => x.IsAlive);
      
            if (!eventInstances.Any()) return ArraySegment<KeyValuePair<MethodInfo, object>>.Empty;
            return eventInstances;
        }

         private class EventsPriorityComparer : IComparer<(EventPriority,int)>
        {
            public int Compare((EventPriority, int) x, (EventPriority, int) y)
            {
                return (y.Item1 - x.Item1) - (y.Item2 - x.Item2);
            }
        } 
    }
}