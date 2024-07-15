using PEzbus;
using PEzBus.Extensions;
using System.Collections.Concurrent;
using System.Reflection;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.MethodInvoker;
using PEzBus.Types;
using PEzBus.Util;


namespace PEzBus.EventBus.Repository
{
    public class EventsRepository : IBusRepository<IPEzEvent,EventPriority>
    {
        private readonly ConcurrentPriorityQueue<IPEzEvent, EventPriority> _eventsQueue = new(new EventsPriorityComparer());
        private readonly ConcurrentDictionary<Type,ReferenceInfo> _eventsInstanceMapping = new();
        private Thread _invokerThread;
        private IPEzMethodInvoker _methodInvoker = new PEzMethodInvoker();
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

        private void HandleEvent(IPEzEvent @event)
        {
             var entries = GetMatchingReferences(@event);
             _methodInvoker.InvokeMethods(entries,@event);
        }

        public void Register<T>(T instance)
        {
            var methods = instance!.GetType().GetMethodsOfInstance<T>();
            foreach (var method in methods)
            {
                var referenceInfo = new ReferenceInfo(method,new WeakReference(instance, true), instance.GetType().Name);
                if(referenceInfo.GetHandledEvent(method, out var handledEvent))
                    _eventsInstanceMapping.TryAdd(handledEvent!, referenceInfo);
            }
        }
        
        public void Publish (IPEzEvent @event, EventPriority priority = EventPriority.HIGH)
        {
            _eventsQueue.TryEnqueue(new KeyValuePair<IPEzEvent, EventPriority>(@event, priority));
        }

         private IEnumerable<KeyValuePair<MethodInfo,object>> GetMatchingReferences(IPEzEvent @event)
        {
            return _eventsInstanceMapping
                    .Where(x => x.Key == @event.GetType() && x.Value.IsAlive)
                    .Select(x => new KeyValuePair<MethodInfo, object>
                        (
                            x.Value.Method,
                            x.Value.Target!
                        )
                    );
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
