using System.Reflection;
using System.Security.Cryptography;
using PEzBus.Extensions;

namespace PEzBus.EventBus.Register;

public sealed class InstanceInfos : IEquatable<InstanceInfos>
{
    public MethodInfo Method { get; }
    private WeakReference Instance { get; }
    public string? InstanceClassName { get; }
    public bool IsAlive => Instance?.IsAlive ?? false;
    public object? Target => IsAlive ? Instance.Target : null;
    public InstanceInfos(in MethodInfo method, WeakReference instance,string? instanceClassName = "")
    {
        Method = method;
        Instance = instance;
        InstanceClassName = instanceClassName;
    }

    public static InstanceInfos FromInstance<T>(T instance, MethodInfo method)
    {
        return new InstanceInfos(method, new WeakReference(instance, false), instance?.GetType().Name);
    }

     public bool GetHandledEvent(out Type? type)
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

    public bool Equals(InstanceInfos other)
    {
        return Instance == other.Instance.Target && other.Method.Name == Method.Name;
    }

    public override int GetHashCode()
    {
        return RandomNumberGenerator.GetInt32(34);
    }

}

