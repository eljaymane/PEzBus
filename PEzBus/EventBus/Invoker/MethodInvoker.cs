using System.Reflection;
using PEzBus.EventBus.Events;

namespace PEzBus.EventBus.Invoker
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
