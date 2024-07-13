using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using PEzbus.CustomAttributes;
using PEzBus.Extensions;
using StructLinq;

namespace PEzBus.EventBus;

public readonly struct ReferenceInfo
{
    private readonly MethodInfo _methodInfo;
    public readonly MethodInfo MethodInfo => _methodInfo;
    private readonly Type? _handledEvent;
    public readonly Type? HandledEvent;

    public ReferenceInfo(in MethodInfo methodInfo)
    {
        _methodInfo = methodInfo;
        GetHandledEvent(methodInfo, out var type);
        _handledEvent = type;
    }

    public bool MatchesEvent(in IPEzEvent @event)
    {
        return _handledEvent == null ? false : _handledEvent == @event.GetType();
    }

    public bool GetHandledEvent(MethodInfo methodInfos, [MaybeNullWhen(false)] out Type? type)
    {
        var subscribeAttribute = _methodInfo.GetSubscribeAttribute();
        if (subscribeAttribute == null)
        {
            type = null;
            return false;
        }
        else
        {
            type = subscribeAttribute.Event;
            return true;
        }
    }

    public override int GetHashCode()
    {
        return RandomNumberGenerator.GetInt32(84);
    }
}

