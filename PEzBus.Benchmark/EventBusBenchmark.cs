using BenchmarkDotNet.Attributes;
using PEzbus.CustomAttributes;
using PEzBus.EventBus;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Queue;

namespace PEzBus.Benchmark;

[ShortRunJob]
[MemoryDiagnoser(true)]
public class EventBusBenchmark
{
    private IEventBus _eventBus = new EventBus.PEzBus();

    [GlobalSetup]
    public void Setup()
    {
        _eventBus = new EventBus.PEzBus();
        for (int i = 0; i < 10; i++)
        {
            var handler = new UserEventsHandler(i);
            var orderHandler = new OrderEventsHandler(i);
            _eventBus.Register(handler);
            _eventBus.Register(orderHandler);
        }
    }

    [Params(1, 1_000,4_000,12_000,25_000,100_000,1_000_000)] public int N;
    [Benchmark]
    public void PublishEvents()
    {
        Parallel.ForEach(Enumerable.Range(0, N), id => _eventBus.Publish(new UserCreatedEvent(N),EventPriority.High));
    }
}
    
   