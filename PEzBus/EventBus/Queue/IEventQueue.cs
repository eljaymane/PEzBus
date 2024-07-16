using PEzBus.EventBus.Events;
using PEzBus.EventBus.Register;

namespace PEzBus.EventBus.Queue;

public interface IEventQueue
{
    void Publish<T>(IEvent @event, EventPriority priority = EventPriority.High);
    void StartHandling(CancellationTokenSource cancellationTokenSource);
    void StopHandling();
}