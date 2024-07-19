using BenchmarkDotNet.Attributes;
using PEzbus.CustomAttributes;
using PEzBus.EventBus;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Queue;
using PEzBus.EventBus.Register;
using PEzBus.EventBus.Register.Abstractions;

namespace PEzBus.Benchmark;

[ShortRunJob]
[MemoryDiagnoser(true)]
public class EventBusBenchmark
{
    private IEventBus _eventBus = new EventBus.PEzBus(default);

    private IInstanceRegister _register = new LocalInstanceRegister();

    [GlobalSetup]
    public void Setup()
    {
        _eventBus = new EventBus.PEzBus(default);

        for (int i = 0; i < 100; i++)
        {
            var handler = new UserEventsHandler(i);
            var orderHandler = new OrderEventsHandler(i);
            _eventBus.Register(handler);
            _eventBus.Register(orderHandler);
        }
    }

    [Params(1, 1_000, 4_000, 12_000, 25_000, 60_000, 120_000)] public int N; 
    [Benchmark]
    public void PublishEvents()
    {
        Parallel.ForEach(Enumerable.Range(0, N), id => _eventBus.Publish(new UserCreatedEvent(N), EventPriority.High));
    }

    //[Params(1, 1_000, 4_000, 12_000, 25_000, 100_000, 1_000_000)] public int id;
    //[Benchmark]
    //public void GetValidInstances()
    //{
    //    for (int i = 0; i < id; i++) {
    //        var result = _register.GetValidInstances(new UserCreatedEvent(i), x => x.IsAlive);
    //        result = _register.GetValidInstances(new OrderUpdatedEvent(3,"KETCHUP"), x => x.IsAlive);
    //    }
       
          
            
            
    }

    
   