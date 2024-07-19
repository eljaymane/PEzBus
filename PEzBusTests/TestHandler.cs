
using PEzbus.CustomAttributes;

namespace PEzBusTest
{
    public class TestHandler
    {
        public string Att { get; set; }
        public TestHandler(string att)
        {
            Att = att;
        }

        [Subscribe(typeof(TestEvent))]
        public void TestMethod(TestEvent testEvent)
        {
            Console.WriteLine("TestMethod : " + testEvent.Message);
        }
        
        public void HereForFun()
        {
            Console.WriteLine("Here for fun : ");
        }
    }

    public class UserHandler
    {
        public string Att { get; set; }
        public UserHandler(string att)
        {
            Att = att;
        }

        [Subscribe(typeof(TestEvent))]
        public void TestMethod(TestEvent testEvent)
        {
            Console.WriteLine("TestMethod : " + testEvent.Message);
        }

        public void HereForFun()
        {
            Console.WriteLine("Here for fun : ");
        }
    }
}
