using System.Collections.Concurrent;
using System.Linq;
using PEzbus.CustomAttributes;

namespace PEzbus
{
    public class PEzEventBus : IPEzEventBus
    {
        private ConcurrentDictionary<Type,WeakReference> _instances;
        public PEzEventBus()
        {
            _instances = new();
        }

        public void Register<T>(T instance)
        {
            _instances.TryAdd(typeof(T), new WeakReference(instance));
        }
        public void Publish(IEzEvent @event)
        {
            //Get methods that has a subscribe attribute with the correct eventType
            var methods = AppDomain.CurrentDomain.GetAssemblies()
                   .SelectMany(x => x.GetTypes())
                   .Where(x => x.IsClass)
                   .SelectMany(x => x.GetMethods())
                   .Where(x => x.GetCustomAttributes(typeof(SubscribeAttribute), false)
                   .FirstOrDefault() != null);
            methods = methods.Where(x =>
                x.GetCustomAttributes(typeof(SubscribeAttribute), false)
                .Any(x => ((SubscribeAttribute)x).Event.Equals(@event.GetType())));

            foreach (var method in methods)
            {
                //Get method parameters
                var param = method.GetParameters()
                   .Where(x => x.ParameterType.Equals(@event.GetType()))
                   .FirstOrDefault();

                //Get types that has the method to be able to filter
                var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .Where(x => x.GetMethods().Contains(method));
                

                    foreach (var entry in _instances.Where(x => types.Contains(x.Key)))
                    {
                        var instance = entry.Value.Target;
                        if (param != null) method.Invoke(instance, new object[] { @event });
                        else method.Invoke(instance, null);
                    }   
            }
            
        }
    }
}
