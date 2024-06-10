using System.Collections.Concurrent;
using EasyBus.CustomAttributes;

namespace EasyBus
{
    public class EventBus
    {
        public EventBus()
        {

        }
        public void Publish(IEvent @event)
        {
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
                var param = method.GetParameters()
                    .Where(x => x.ParameterType.Equals(@event.GetType()))
                    .FirstOrDefault();
                var obj = Activator.CreateInstance(method.DeclaringType);
                if(param !=null) method.Invoke(obj, new object[] { @event });
                else method.Invoke(obj, null);
            }
        }
    }
}
