using System.Reflection;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.MethodInvoker;

namespace PEzBus.EventBus.Repository
{
    public interface IBusRepository<TElement, TPriority>
    {
        void Register<T>(T instance);
        void Publish(TElement @event, TPriority priority);
    }
} 
