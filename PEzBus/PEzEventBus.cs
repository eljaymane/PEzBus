using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using StructLinq;
using PEzbus.CustomAttributes;

namespace PEzbus;
    public class PEzEventBus : IPEzEventBus
    {
        private ConcurrentDictionary<TypeInfo,WeakReference> _instances;
        public PEzEventBus()
        {
            _instances = new();
        }

        public void Register<T>(T instance)
        {
            var methods = typeof(T).GetMethods()
                .Where(methodInfo => methodInfo.GetCustomAttributes()
                    .Any(x => x.GetType() == Global.SubscribeAttributeType));
            
            var typeInfo = new TypeInfo(methods.ToList());
            _instances.TryAdd(typeInfo, new WeakReference(instance,true));
        }

        public void Publish(IPEzEvent @event)
        {
            var entries = _instances
                .Where(x => x.Value.IsAlive)
                .Select(x => new
            {
                Methods =  x.Key.GetMatchingMethods(@event),
                Instance = x.Value.Target
            });

            foreach (var entry in entries)
            {
                Parallel.ForEach(entry.Methods, method =>
                {
                    if (method.GetParameters().Any(x => @event.GetType() == x.ParameterType))
                        method.Invoke(entry.Instance, [@event]);
                    else method.Invoke(entry.Instance, null);
                });
            }
        }
        
    }

