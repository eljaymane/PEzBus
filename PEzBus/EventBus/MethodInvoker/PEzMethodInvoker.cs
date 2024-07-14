using System.Reflection;
using PEzBus.EventBus.Events;

namespace PEzBus.EventBus.MethodInvoker
{
    public class PEzMethodInvoker : IPEzMethodInvoker
    {
        public void InvokeMethods(in IEnumerable<KeyValuePair<MethodInfo, object>> methods, IPEzEvent @event)
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
