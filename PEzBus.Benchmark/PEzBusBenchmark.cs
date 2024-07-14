using BenchmarkDotNet.Attributes;
using PEzbus.CustomAttributes;
using PEzbus.EventBus;
using PEzBus.EventBus;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.MethodInvoker;

namespace PEzBus.Benchmark;

[ShortRunJob]
[MemoryDiagnoser(false)]
public class PEzBusBenchmark
{
    private IPEzEventBus _eventBus = new PEzEventBus(new PEzMethodInvoker());

    [GlobalSetup]
    public void Setup()
    {
        _eventBus = new PEzEventBus(new PEzMethodInvoker());
        for (int i = 0; i < 10 ; i++)
        {
            var handler = new TestEventHandler(i);
            _eventBus.Register(handler);
        }
           
    }

    [Params(1, 1_000,4_000,12_000,25_000,100_000,1_000_000)] public int N;
    [Benchmark]
    public void PublishEvents()
    {
        Parallel.ForEach(Enumerable.Range(0, N), id => _eventBus.Publish(new TestEvent(N)));
    }

}
    
    public class TestEvent : IPEzEvent
    {
        public int Id { get; }
        public string? Argument { get; set; }
        public TestEvent(int id)
        {
            Id = id;
        }
    }

    public sealed class TestEventHandler
    {
        public int Id { get; set; }

    public TestEventHandler(int id)
    {
        Id = id;
    }

    [Subscribe(typeof(TestEvent))]
        public void HandleTestEventOne(TestEvent testEvent)
        {
            //testEvent.Argument = "voil�";
            //Console.WriteLine($"Handler one : {testEvent.Argument}");
        }

        [Subscribe(typeof(TestEvent))]
        public void HandleTestEventTwo(TestEvent testEvent)
        {
            //testEvent.Argument = "voil�";
        }
    }