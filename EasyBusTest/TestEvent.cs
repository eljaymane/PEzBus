using PEzbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBusTest
{
    public class TestEvent : IEzEvent
    {
        public string Message { get; set; }
        public TestEvent()
        {
                
        }

        public TestEvent(string message)
        {
            Message = message;
        }
    }
}
