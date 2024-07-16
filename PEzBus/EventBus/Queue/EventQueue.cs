using PEzBus.EventBus.Events;
using PEzBus.EventBus.Invoker;
using PEzBus.EventBus.Register;
using PEzBus.Types;
using PEzBus.Util;

namespace PEzBus.EventBus.Queue;

public class EventQueue : IEventQueue
{
    private Thread _invokerThread;
    private readonly IMethodInvoker _methodInvoker = new Invoker.MethodInvoker();
    private readonly ConcurrentPriorityQueue<IEvent, EventPriority> _eventsQueue = new(new EventsPriorityComparer());
    private readonly IInstancesRegister? _registerContext;

    public EventQueue(IInstancesRegister? registerContext)
    {
        _registerContext = registerContext;
    }

    public IEvent GetNext()
    {
         _eventsQueue.TryDequeue(out var item);
         return item.Key;
    }

    public void Publish<T>(IEvent @event, EventPriority priority = EventPriority.High)
    {
        _eventsQueue.TryEnqueue(new KeyValuePair<IEvent, EventPriority>(@event, priority));
    }

    public void StartHandling(CancellationTokenSource cancellationTokenSource)
    {
        _invokerThread = BackgroundThread.Start(HandleEvents);
        cancellationTokenSource.Token.Register(() => _invokerThread.Interrupt());
    }

    public void StopHandling()
    {
        if(_invokerThread.ThreadState.Equals(ThreadState.Background))
            _invokerThread.Interrupt();
    }

    private void HandleEvents()
    {
        while (true)
        {
            if(_eventsQueue.TryDequeue(out var item)) 
                    HandleEvent(item.Key);
        }
    }
    
    
    private void HandleEvent(IEvent @event)
    {
         var entries = _registerContext.GetValidInstances(@event, x => x.IsAlive);
         _methodInvoker.InvokeMethods(entries,@event);
    }


   private class EventsPriorityComparer : IComparer<(EventPriority,int)>
    {
        public int Compare((EventPriority, int) x, (EventPriority, int) y)
        {
            return (y.Item1 - x.Item1) - (y.Item2 - x.Item2);
        }
    }

    
}
