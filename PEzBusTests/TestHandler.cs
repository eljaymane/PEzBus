using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEzBus.Attributes;

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
}
