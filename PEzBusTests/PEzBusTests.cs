using System.Linq;
using NuGet.Frameworks;
using System.Runtime.CompilerServices;
using PEzBus.Attributes;
using PEzBus.EventBus;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Repository;
using PEzbus.EventsPubSub;

namespace PEzBusTest
{
    [TestClass]
    public class PEzBusTests
    {
        private IEventBus eventBus;
        private int _CallNumbers;

        [TestInitialize]
        public void Initialize()
        {
            eventBus = new PEzEventBus();
            _CallNumbers = 0;
        }
        [TestMethod]
        public void Should_Call_Method_When_Event_IsPublished()
        {
            eventBus.Register(this);
            eventBus.Publish(new TestEvent(),EventPriority.High);
        }

        [TestMethod]
        public void Should_Call_Method_With_Args_When_Event_Is_Published()
        {
            TestHandler test = new ("teesst");
            eventBus.Register(test);
            TestHandler test2 = new ("teesst");
            eventBus.Register(test2);
            eventBus.Publish(new TestEvent("Called you maybe"),EventPriority.High);
            var testEvents = from testIndex in Enumerable.Range(0,10)
                                            select new TestEvent(testIndex.ToString());

            foreach (var testEvent in testEvents)
            {
                eventBus.Publish(testEvent,EventPriority.High);
            }
           
        }

        [TestMethod]
        public void Shoudl_Call_Methods_When_Multiple_Events_AreHandled()
        {
            TestHandler test = new("teesst");
            eventBus.Register(test);
            eventBus.Publish(new TestEvent("Called you maybe"),EventPriority.High);
            IEnumerable<IEvent> testEvents = from testIndex in Enumerable.Range(0, 10)
                             select new TestEvent(testIndex.ToString());
            IEnumerable<IEvent> testEvents2 = from testIndex in Enumerable.Range(0, 10)
                             select new AnotherTestEvent((testIndex.ToString()));
            testEvents = testEvents.Concat(testEvents);

            Parallel.ForEach(testEvents, testEvent =>
            {
                eventBus.Publish(testEvent,EventPriority.High);
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
        
        [Subscribe(typeof(TestEvent))]
        public void callMeaybes(TestEvent @event)
        {
            Console.WriteLine("Another call : " + @event.Message);
        }
    }
}