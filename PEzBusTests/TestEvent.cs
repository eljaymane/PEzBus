﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEzBus.EventBus.Events;

namespace PEzBusTest
{
    public class TestEvent : IEvent
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
