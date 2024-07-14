using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using PEzbus.CustomAttributes;
using PEzBus.EventBus.Events;
using PEzBus.Extensions;
using StructLinq;

namespace PEzBus.EventBus.Repository;

public readonly struct ReferenceInfo : IEquatable<ReferenceInfo>
{
    public MethodInfo Method { get; }
    private readonly WeakReference Instance { get; }
    public string? InstanceClassName { get; }
    public bool IsAlive => Instance?.IsAlive ?? false;
    public object? Target => IsAlive ? Instance.Target : null;
    public ReferenceInfo(in MethodInfo method, WeakReference instance,string? instanceClassName = "")
    {
        InstanceClassName = instanceClassName;
        Method = method;
        Instance = instance;
    }

     public bool GetHandledEvent(MethodInfo methodInfos, [MaybeNullWhen(false)] out Type? type)
    {
        var subscribeAttribute = Method.GetSubscribeAttribute();
        if (subscribeAttribute == null)
        {
            type = null;
            return false;
        }

        type = subscribeAttribute.Event;
        return true;
    }

    public bool Equals(ReferenceInfo other)
    {
        return Instance == other.Instance && other.Method == Method;
    }

    public override int GetHashCode()
    {
        return RandomNumberGenerator.GetInt32(34);
    }
}

