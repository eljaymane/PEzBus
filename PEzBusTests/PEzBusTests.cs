using PEzbus.CustomAttributes;
using System.Linq;
using NuGet.Frameworks;
using System.Runtime.CompilerServices;
using PEzBus.EventBus;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.MethodInvoker;

namespace PEzBusTest
{
    [TestClass]
    public class PEzBusTests
    {
        private PEzEventBus eventBus;
        private int _CallNumbers;

        [TestInitialize]
        public void Initialize()
        {
            eventBus = new(new PEzMethodInvoker());
            _CallNumbers = 0;
        }
        [TestMethod]
        public void Should_Call_Method_When_Event_IsPublished()
        {
            eventBus.Register(this);
            eventBus.Publish(new TestEvent());
        }

        [TestMethod]
        public void Should_Call_Method_With_Args_When_Event_Is_Published()
        {
            TestClass test = new ("teesst");
            eventBus.Register(test);
            eventBus.Publish(new TestEvent("Called you maybe"));
            var testEvents = from testIndex in Enumerable.Range(0,10)
                                            select new TestEvent(testIndex.ToString());

            foreach (var testEvent in testEvents)
            {
                eventBus.Publish(testEvent);
            }
           
        }

        [TestMethod]
        public void Shoudl_Call_Methods_When_Multiple_Events_AreHandled()
        {
            TestClass test = new("teesst");
            eventBus.Register(test);
            eventBus.Publish(new TestEvent("Called you maybe"));
            IEnumerable<IPEzEvent> testEvents = from testIndex in Enumerable.Range(0, 10)
                             select new TestEvent(testIndex.ToString());
            IEnumerable<IPEzEvent> testEvents2 = from testIndex in Enumerable.Range(0, 10)
                             select new AnotherTestEvent((testIndex.ToString()));
            testEvents = testEvents.Concat(testEvents);

            Parallel.ForEach(testEvents, testEvent =>
            {
                eventBus.Publish(testEvent);
            });
        }


        [Subscribe(typeof(TestEvent))]
        public void callMeMaybe()
        {
            Console.WriteLine("Called you baby");
            _CallNumbers++;
        }

        [Subscribe(typeof(TestEvent))]
        public void callMeMaybeArgs(TestEvent @event)
        {
            Console.WriteLine(@event.Message);
        }
    }
}