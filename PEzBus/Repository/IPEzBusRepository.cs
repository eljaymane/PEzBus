using PEzBus.EventBus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PEzBus.Repository
{
    public interface IPEzBusRepository
    {
        void Register<T>(T instance);
        IEnumerable<KeyValuePair<ReferenceInfo, WeakReference>> GetMatchingReferences(IPEzEvent @event);
        void Cleanup();
    }
} 
