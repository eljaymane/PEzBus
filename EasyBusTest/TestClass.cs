using PEzbus.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEzBusTest
{
    public class TestClass
    {
        public string Att { get; set; }
        public TestClass(string att)
        {
            Att = att;
        }

        [Subscribe(typeof(TestEvent))]
        public void TestMethod(TestEvent testEvent)
        {
            Console.WriteLine("TestMethod : " + testEvent.Message);
        }
    }
}
