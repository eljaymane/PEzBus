using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using PEzbus.CustomAttributes;
using StructLinq;

namespace PEzbus;

public readonly struct TypeInfo
{
    private readonly Dictionary<MethodInfo,IEnumerable<Type>> _handledEvents;
    // private readonly IReadOnlyList<IPEzEvent> _handledEvents;

    public TypeInfo(in IReadOnlyList<MethodInfo> methodInfos)
    {
        _handledEvents = GetHandledEvents(methodInfos);
    }

    public Dictionary<MethodInfo,IEnumerable<Type>> GetHandledEvents(IReadOnlyList<MethodInfo> methodInfos)
    {
        var result = new Dictionary<MethodInfo, IEnumerable<Type>>();
        foreach (var method in methodInfos)
        {
            result.AddOrUpdate(method, method.GetCustomAttributes()
                .Select(x => ((SubscribeAttribute)x).Event));
        }
        return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<MethodInfo> GetMatchingMethods(IPEzEvent @event)
    {
            var methods = _handledEvents
                .Where(x => x.Value.Contains(@event.GetType()))
                .Select(x => x.Key);
            return methods;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AnyMatchingMethods(IPEzEvent @event)
    {
        return _handledEvents.Values.Any(x => x == @event.GetType());
    }

    public override int GetHashCode()
    {
        return _handledEvents.GetHashCode() - RandomNumberGenerator.GetInt32(84);
    }
}

