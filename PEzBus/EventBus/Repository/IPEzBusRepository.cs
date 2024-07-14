using System.Reflection;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.MethodInvoker;

namespace PEzBus.EventBus.Repository
{
    public interface IPEzBusRepository
    {
        void Register<T>(T instance);
        IEnumerable<KeyValuePair<MethodInfo, object>> GetMatchingReferences(IPEzEvent @event);
        void Cleanup();
    }
} 
