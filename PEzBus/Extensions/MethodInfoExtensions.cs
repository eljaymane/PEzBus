using StructLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PEzBus.Extensions
{
    public static class MethodInfoExtensions
    {
        public static SubscribeAttribute? GetSubscribeAttribute(this MethodInfo methodInfo)
        {
            if (methodInfo == null) goto EXIT;
            else if (methodInfo.GetCustomAttributes().Any(x => x.GetType() == typeof(SubscribeAttribute)))
                return methodInfo.GetCustomAttribute(typeof(SubscribeAttribute)) as SubscribeAttribute;
        EXIT:
            return null;
        }
    }
}
