using System.Reflection;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Queue;

namespace PEzBus.EventBus.Register;

public interface IInstancesRegister : IRegister<IEvent,EventPriority>
{
    IEnumerable<KeyValuePair<MethodInfo, object>> GetValidInstances(IEvent @event, Func<InstanceInfos, bool> predicate);
}