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
    }
    
    [Params(1, 4000)] public int N;
    [Benchmark]
    public void PublishEvents()
    {
        _eventBus.Publish(new TestEvent(N.ToString()));
    }
    
    public class TestEvent : IPEzEvent
    {
        public string Argument;
        public TestEvent(string argument)
        {
            Argument = argument;
        }
    }

    public class TestEventHandler : IPEzEvent
    {
        [Subscribe(typeof(TestEvent))]
        public void HandleTestEvent(TestEvent @event)
        {
            Console.WriteLine("Test working : {id} " ,@event.Argument);
        }
    }
}