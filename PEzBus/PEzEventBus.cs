using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using StructLinq;
using PEzbus.CustomAttributes;

namespace PEzbus
{
    public class PEzEventBus : IPEzEventBus
    {
        private ConcurrentDictionary<TypeInfo,WeakReference> _instances;
        public PEzEventBus()
        {
            _instances = new();
        }

        public void Register<T>(T instance)
        {
            var typeInfo = new TypeInfo(typeof(T), typeof(T).GetMethods());
            _instances.TryAdd(typeInfo, new WeakReference(instance));
        }

        private IEnumerable<WeakReference> GetInstancesByType(Type type)
        {
            return _instances.Values.Where(x => x.Target.GetType() == type);
        }

        public void Publish(IPEzEvent @event)
        {
            var entries = _instances.Keys.Select(x => new
            {
                @Type = x.@Type,
                Methods =  x.GetMatchingMethods(@event)
            });
            foreach (var entry in entries)
            {
                var instances = GetInstancesByType(entry.@Type);
                foreach (var method in entry.Methods)
                {
                    foreach (var instance in instances)
                    {
                        if (method.GetParameters().Any(x => @event.GetType() == x.ParameterType))
                            method.Invoke(instance.Target, new object[] { @event });
                        else method.Invoke(instance.Target, null);
                    }

                    
                }
            }
        }
    }
}
