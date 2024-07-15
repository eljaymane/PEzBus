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

public sealed class EventsInfos : IDisposable, IEquatable<EventsInfos>
{
    public MethodInfo Method { get; }
    private WeakReference Instance { get; }
    public string? InstanceClassName { get; }
    public bool IsAlive => Instance?.IsAlive ?? false;
    public object? Target => IsAlive ? Instance.Target : null;
    public EventsInfos(in MethodInfo method, WeakReference instance,string? instanceClassName = "")
    {
        InstanceClassName = instanceClassName;
        Method = method;
        Instance = instance;
    }

    public static EventsInfos FromInstance<T>(T instance, MethodInfo method)
    {
        return new EventsInfos(method, new WeakReference(instance, false), instance.GetType().Name);
    }

     ~EventsInfos()
     {
         Instance.Target = null;
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

    public bool Equals(EventsInfos other)
    {
        return Instance == other.Instance && other.Method == Method;
    }

    public override int GetHashCode()
    {
        return RandomNumberGenerator.GetInt32(34);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool _disposedValue;
    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Instance.Target = null;
            }

            _disposedValue = true;
        }
    }
    
}

