using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PEzBus.EventBus.Events;

namespace PEzBus.EventBus.MethodInvoker
{
    public interface IPEzMethodInvoker
    {
        void InvokeMethods(in IEnumerable<KeyValuePair<MethodInfo,object>> methods,IPEzEvent @event);
    }
}
