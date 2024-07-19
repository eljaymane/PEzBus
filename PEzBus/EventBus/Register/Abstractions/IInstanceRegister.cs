using System.Collections.Concurrent;
using System.Reflection;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Queue;

namespace PEzBus.EventBus.Register.Abstractions;

public interface IInstanceRegister : IRegister<IEvent, EventPriority>
{
    ConcurrentDictionary<Type, List<InstanceInfos>> GetAll();
    IEnumerable<KeyValuePair<MethodInfo, object>> GetValidInstances(IEvent @event, Func<InstanceInfos, bool> predicate);
}