﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEzBus.EventBus
{
    public interface IPezMethodInvoker
    {
        void InvokeMethods(IEnumerable<KeyValuePair<ReferenceInfo,WeakReference>> methods, IPEzEvent @event);
    }
}
