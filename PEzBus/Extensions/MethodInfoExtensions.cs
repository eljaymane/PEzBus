using StructLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PEzBus.Extensions;
    public static class MethodInfoExtensions
    {
        public static SubscribeAttribute? GetSubscribeAttribute(this MethodInfo methodInfo) 
            => methodInfo.GetCustomAttribute(Global.SubscribeAttributeType) as SubscribeAttribute;
    }

