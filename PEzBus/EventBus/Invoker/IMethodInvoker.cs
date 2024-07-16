using System.Reflection;
using PEzBus.EventBus.Events;

namespace PEzBus.EventBus.Invoker
{
    public interface IMethodInvoker
    {
        void InvokeMethods(in IEnumerable<KeyValuePair<MethodInfo,object>> methods,IEvent @event);
    }
}
