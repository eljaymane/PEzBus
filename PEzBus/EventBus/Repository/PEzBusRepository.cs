﻿using PEzbus;
using PEzBus.Extensions;
using System.Collections.Concurrent;
using System.Reflection;
using PEzBus.EventBus.Events;


namespace PEzBus.EventBus.Repository
{
    public class PEzBusRepository : IPEzBusRepository
    {
        private readonly ConcurrentDictionary<Type,ReferenceInfo> _eventsInstanceMapping = new();

        public void Register<T>(T instance)
        {
            var methods = instance!.GetType().GetMethodsOfInstance<T>();
            foreach (var method in methods)
            {
                var referenceInfo = new ReferenceInfo(method,new WeakReference(instance, true), instance.GetType().Name);
                referenceInfo.GetHandledEvent(method, out var handledEvent);
                _eventsInstanceMapping.TryAdd(handledEvent!, referenceInfo);
            }
        }

        public IEnumerable<KeyValuePair<MethodInfo,object>> GetMatchingReferences(IPEzEvent @event)
        {
            return _eventsInstanceMapping
                    .Where(x => x.Value.IsAlive && x.Key == @event.GetType())
                    .Select(x => new KeyValuePair<MethodInfo, object>
                        (
                            x.Value.Method,
                            x.Value.Target!
                        )
                    );
    }

        public void Cleanup()
        {
                _eventsInstanceMapping.RemoveAll<Type, ReferenceInfo>(entry 
                    => !entry.Value.IsAlive);
        }
    }
}
