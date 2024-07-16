using PEzbus.CustomAttributes;
using PEzBus.EventBus;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Queue;
using PEzBus.EventBus.Register;
using PEzBusTest;

namespace PEzBusTests
{
    [TestClass]
    public class PEzBusTests
    {
        private IEventBus _eventBus;
        private int _callNumbers;

        [TestInitialize]
        public void Initialize()
        {
            _eventBus = new PEzBus.EventBus.PEzBus();
            _callNumbers = 0;
        }
        [TestMethod]
        public void Should_Call_Method_When_Event_IsPublished()
        {
            _eventBus.Register(this);
            _eventBus.Publish(new TestEvent(),EventPriority.High);
        }

        [TestMethod]
        public void Should_Call_Method_With_Args_When_Event_Is_Published()
        {
            TestHandler test = new ("teesst");
            _eventBus.Register(test);
            TestHandler test2 = new ("teesst");
            _eventBus.Register(test2);
            _eventBus.Publish(new TestEvent("Called you maybe"),EventPriority.High);
            var testEvents = from testIndex in Enumerable.Range(0,10)
                                            select new TestEvent(testIndex.ToString());

            foreach (var testEvent in testEvents)
            {
                _eventBus.Publish(testEvent,EventPriority.High);
            }
           
        }

        [TestMethod]
        public void Shoudl_Call_Methods_When_Multiple_Events_AreHandled()
        {
            TestHandler test = new("teesst");
            _eventBus.Register(test);
            _eventBus.Publish(new TestEvent("Called you maybe"),EventPriority.High);
            IEnumerable<IEvent> testEvents = from testIndex in Enumerable.Range(0, 10)
                             select new TestEvent(testIndex.ToString());
            IEnumerable<IEvent> testEvents2 = from testIndex in Enumerable.Range(0, 10)
                             select new AnotherTestEvent((testIndex.ToString()));
            testEvents = testEvents.Concat(testEvents);

            Parallel.ForEach(testEvents, testEvent =>
            {
                _eventBus.Publish(testEvent,EventPriority.High);
            });
        }


        [Subscribe(typeof(TestEvent))]
        public void callMeMaybe()
        {
            Console.WriteLine("Called you baby");
            _callNumbers++;
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