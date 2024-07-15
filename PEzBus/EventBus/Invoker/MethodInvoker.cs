using System.Reflection;
using PEzBus.EventBus.Events;
using PEzBus.EventsPubSub.Invoker;

namespace PEzBus.EventBus.MethodInvoker
{
    public class MethodInvoker : IMethodInvoker
    {
        public void InvokeMethods(in IEnumerable<KeyValuePair<MethodInfo, object>> methods, IEvent @event)
        {
            foreach (var method in methods)
            {
                if (method.Key.GetParameters().Any(x => @event.GetType() == x.ParameterType))
                    method.Key.Invoke(method.Value, [@event]);
                else method.Key.Invoke(method.Value, null);
            }
        }
    }
}
