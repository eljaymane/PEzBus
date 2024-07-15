using System.Reflection;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Repository;

namespace PEzBus.EventsPubSub.Repository;

public interface IEventsRepository : IRepository<IEvent,EventPriority>
{
    IEnumerable<KeyValuePair<MethodInfo, object>> GetValidInstances(IEvent @event, Func<EventsInfos, bool> predicate);
}