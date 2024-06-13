using PEzbus;
using PEzbus.CustomAttributes;
using NuGet.Frameworks;
using System.Runtime.CompilerServices;

namespace EasyBusTest
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
            eventBus.Register<EventBusTests>(this);
            eventBus.Publish(new TestEvent());
        }

        [TestMethod]
        public void Should_Call_Method_With_Args_When_Event_Is_Published()
        {
            eventBus.Register<EventBusTests>(this);
            eventBus.Publish(new TestEvent("Called you maybe"));
            eventBus.Publish(new TestEvent("1"));
            eventBus.Publish(new TestEvent("2"));
            eventBus.Publish(new TestEvent("3"));
            eventBus.Publish(new TestEvent("4"));
            eventBus.Publish(new TestEvent("5"));
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