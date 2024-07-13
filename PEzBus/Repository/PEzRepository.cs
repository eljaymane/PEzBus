using PEzbus;
using PEzBus.EventBus;
using PEzBus.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace PEzBus.Repository
{
    public class PEzRepository : IPEzBusRepository
    {
        private ConcurrentDictionary<ReferenceInfo, WeakReference> _EventsInstanceMapping;

        public ConcurrentDictionary<ReferenceInfo, WeakReference> EventsInstanceMapping { get => _EventsInstanceMapping; }

        public PEzRepository()
        {
            _EventsInstanceMapping = new();
        }

       public void Register<T>(T  instance)
        {
            var methods = instance!.GetType().GetMethodsOfInstance<T>();
            foreach (var method in methods)
            {
                var typeInfo = new ReferenceInfo(method);
                _EventsInstanceMapping.TryAdd(typeInfo, new WeakReference(instance, true));
            }
        }

        public IEnumerable<KeyValuePair<ReferenceInfo, WeakReference>> GetMatchingReferences(IPEzEvent @event)
        {
            return _EventsInstanceMapping
            .Where(x => x.Value.IsAlive && x.Key.MatchesEvent(@event));
        }

        public void Cleanup()
        {
            _EventsInstanceMapping.RemoveAll<ReferenceInfo, WeakReference>(entry => entry.Key.HandledEvent == null || !entry.Value.IsAlive);
        }
    }
}
