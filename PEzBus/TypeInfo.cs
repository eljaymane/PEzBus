using System.Reflection;
using PEzbus.CustomAttributes;
using StructLinq;

namespace PEzbus;

public readonly struct TypeInfo
{
    public readonly Type @Type;
    public readonly MethodInfo[] MethodInfos;

    public TypeInfo(Type type, MethodInfo[] methodInfos)
    {
        @Type = type;
        MethodInfos = methodInfos;
    }

    public IEnumerable<MethodInfo> GetMatchingMethods(IPEzEvent @event)
    {
        try
        {
            var methods = MethodInfos.Where(x => x.GetCustomAttributes()
                .Where(x => x.GetType() == typeof(SubscribeAttribute))
                .Any(x => ((SubscribeAttribute)x).Event == @event.GetType()));
            return methods;
        }
        catch (Exception)
        {
            return Array.Empty<MethodInfo>();
        }
    }
}
