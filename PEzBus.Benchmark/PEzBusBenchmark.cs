using BenchmarkDotNet.Attributes;
using PEzbus;
using PEzbus.CustomAttributes;

namespace PEzBus.Benchmark;

[ShortRunJob]
[MemoryDiagnoser(false)]
public class PEzBusBenchmark
{
    private IPEzEventBus _eventBus;

    [GlobalSetup]
    public void Setup()
    {
        _eventBus = new PEzEventBus();
        var handler = new TestEventHandler();
        _eventBus.Register(handler);
    }

    [Params(1, 4_000,50_000,100_000,1_000_000)] public int N;
    [Benchmark]
    public void PublishEvents()
    {
        var ids = Enumerable.Range(0, N);
        Parallel.ForEach(ids, id => _eventBus.Publish(new TestEvent(N)));
    }

}
    
    public class TestEvent : IPEzEvent
    {
        public int Id {  get; set; }
        public string Argument { get; set; }
        public TestEvent(int id)
        {
            Id = id;
        }
    }

    public class TestEventHandler
    {
        [Subscribe(typeof(TestEvent))]
        public void HandleTestEventOne(TestEvent testEvent)
        {
            testEvent.Argument = "voilà";
            //Console.WriteLine($"Handler one : {testEvent.Argument}");
        }

        [Subscribe(typeof(TestEvent))]
        public void HandleTestEventTwo(TestEvent testEvent)
        {
            testEvent.Argument = "voilà";
        }
    }