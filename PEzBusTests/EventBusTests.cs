using PEzbus;
using PEzbus.CustomAttributes;
using System.Linq;
using NuGet.Frameworks;
using System.Runtime.CompilerServices;

namespace PEzBusTest
{
    [TestClass]
    public class EventBusTests
    {
        private PEzEventBus eventBus;
        private int _CallNumbers;

        [TestInitialize]
        public void Initialize()
        {
            eventBus = new();
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