using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEzBus.EventBus
{
    public class PEzMethodInvoker : IPezMethodInvoker
    {
        public void InvokeMethods(IEnumerable<KeyValuePair<ReferenceInfo, WeakReference>> methods, IPEzEvent @event)
        {
            foreach (var method in methods)
            {
                if (method.Key.MethodInfo.GetParameters().Any(x => @event.GetType() == x.ParameterType))
                    method.Key.MethodInfo.Invoke(method.Value.Target, [@event]);
                else method.Key.MethodInfo.Invoke(method.Value.Target, null);
            }
        }
    }
}
