using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PEzBus.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<MethodInfo> GetMethodsOfInstance<T>([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] this Type @type)
        {
            return type.GetMethods()
                    .Where(methodInfo => methodInfo.GetCustomAttributes()
                        .Any(x => x.GetType() == Global.SubscribeAttributeType));
        }
    }
}
